using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;
using ShopOnline.Models.CreateModels;
using ShopOnline.Models.EditModels;
using ShopOnline.Repository;
using Microsoft.AspNetCore.Identity;
using ShopOnline.Models.ViewModels;
using System.Drawing.Printing;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using ShopOnline.Hubs;

namespace ShopOnline.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: ProductController
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IHubContext<UpdateOrderHub> _orderhubs;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private const int PRODUCT_PER_PAGE = 3;

        public ProductController(ApplicationDbContext context, IProductRepository productRepository,
                                             ILogger<ProductController> logger, IProductCategoryRepository productCategoryRepository,
                                             ITagRepository tagRepository,
                                             IMapper mapper,
                                             UserManager<AppUser> userManager, IHubContext<UpdateOrderHub> orderhubs,
                                             IWebHostEnvironment environment, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _logger = logger;
            _productCategoryRepository = productCategoryRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
            _userManager = userManager;
            _environment = environment;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderhubs= orderhubs;
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(int? pageNumber)
        {
            var productViewModel = new ViewProductModel();
            var categories = await _productCategoryRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            var totalPage = products.Count > 0 ? (products.Count - 1) / PRODUCT_PER_PAGE + 1 : 0;
            int pageNumberValue = pageNumber ?? 1;
            var productsInPage = products
        .Skip((pageNumberValue - 1) * PRODUCT_PER_PAGE)
        .Take(PRODUCT_PER_PAGE)
        .ToList();
            productViewModel.Products = productsInPage;
            productViewModel.TotalPages = totalPage;
            productViewModel.PageNumber = pageNumberValue;
            productViewModel.CreateOrUpdate = new CreateProductDto
            {
                ListCategories = (List<ProductCategory>)categories,
                ListTags = (List<Tag>)tags
            };
            return View(productViewModel);
        }

        [AllowAnonymous]
        public async Task<ActionResult> CustomerView(int? pageNumber, int? categoryId, string? searchKey, string? orderBy,
                     decimal? minPrice = 0, decimal? maxPrice = 2000)
        {
            var productViewModel = new ViewCustomerProductModel();

            var products = await _productRepository.GetAllAsync();

            var topSixNewestProducts =await _productRepository.TopSixNewestProducts();
            var productCategories = await _productCategoryRepository.GetAllAsync();
            if (categoryId != null) products = products.Where(p => p.CategoryID == categoryId.Value).ToList();
            if (!string.IsNullOrEmpty(searchKey))
            {
                searchKey = searchKey.ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(searchKey) || p.Description.ToLower().Contains(searchKey)
                                                || p.Alias.ToLower().Contains(searchKey)).ToList();
            }
            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderBy.ToLower().Equals("asc")) products = products.OrderBy(p => p.Price).ToList();
                else products = products.OrderByDescending(p => p.Price).ToList();
            }
            var totalPage = products.Count > 0 ? (products.Count - 1) / PRODUCT_PER_PAGE + 1 : 0;
            int pageNumberValue = pageNumber ?? 1;
            var productsInPage = products.Skip((pageNumberValue - 1) * PRODUCT_PER_PAGE)
                                         .Take(PRODUCT_PER_PAGE)
                                         .ToList();
            productViewModel.Products = productsInPage;
            productViewModel.TotalPages = totalPage;
            productViewModel.PageNumber = pageNumberValue;
            productViewModel.SearchKey = searchKey;
            productViewModel.CategoryId = categoryId;
            productViewModel.MinPrice = minPrice;
            productViewModel.MaxPrice = maxPrice;
            productViewModel.OrderBy = orderBy;
            productViewModel.Top6NewestProducts = topSixNewestProducts;
            productViewModel.ProductCategories =(List<ProductCategory>) productCategories;
            return View(productViewModel);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: ProductController/Create
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create()
        {
            var categories = await _productCategoryRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();
            var viewModel = new CreateProductDto
            {
                ListCategories = (List<ProductCategory>)categories,
                ListTags = (List<Tag>)tags
            };

            return View(viewModel);
        }


        // POST: ProductController/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(CreateProductDto productDto)
        {
            string fileImage = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    if (user != null)
                    {
                        string fileExtension = Path.GetExtension(productDto.ImageUpload.FileName);
                        string fileName = $"{user.Id}_{DateTime.Now.Ticks}_{productDto.ImageUpload.FileName}";
                        fileImage = fileName;
                        string filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productDto.ImageUpload.CopyToAsync(fileStream);
                        }

                        var product = _mapper.Map<Product>(productDto);
                        product.CreatedDate = DateTime.UtcNow;
                        product.CreatedBy = user.UserName;
                        product.UpdatedDate = DateTime.UtcNow;
                        product.UpdatedBy = user.UserName;
                        product.Image = fileName;
                        product.ViewCount = 0;

                        if (productDto.ListSelectedTags != null)
                        {
                            foreach (var tagId in productDto.ListSelectedTags)
                            {
                                var productTag = new ProductTag
                                {
                                    ProductID = product.ID,
                                    TagID = tagId
                                };
                                product.ProductTags.Add(productTag);
                            }
                        }

                        await _productRepository.AddAsync(product);
                        await _productRepository.SaveAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                _logger.LogError(ex, "An error occurred while creating the product.");

                // Add an error message to ModelState
                ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");

                // Delete the file if it exists
                if (!string.IsNullOrEmpty(fileImage))
                {
                    string filePath = Path.Combine(_environment.WebRootPath, "uploads", fileImage);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            var categories = await _productCategoryRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();
            productDto.ListCategories = (List<ProductCategory>)categories;
            productDto.ListTags = (List<Tag>)tags;

            return View(productDto);
        }


        [AllowAnonymous]
        public async Task<ActionResult> ProductDetailView(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            var productDto = _mapper.Map<ViewProductDetailModel>(product);
            return View(productDto);
        }
        // GET: ProductController/Edit/5
        public async Task<PartialViewResult> Edit(int id)
        {
            var model = await _productRepository.GetProductByIdWithListTagsAsync(id);
            var modelDto = _mapper.Map<EditProductDto>(model);
            modelDto.ListCategories = (List<ProductCategory>) await _productCategoryRepository.GetAllAsync();
            modelDto.ListTags = (List<Tag>) await _tagRepository.GetAllAsync();
            modelDto.ListSelectedTags = new List<string>();
            foreach (var item in model.ProductTags)
            {
                modelDto.ListSelectedTags.Add(item.TagID);
            }
            ViewBag.Image = model.Image;
            ViewBag.Id = model.ID;
            return PartialView("_EditProduct" , modelDto);
        }

        // POST: ProductController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int productid, EditProductDto createOrUpdate)
        {
            string fileImage = "";
            var existingProduct = await _productRepository.GetProductByIdWithListTagsAsync(productid);
            if (existingProduct == null) return NotFound();
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the existing product from the repository
                    

                    if (existingProduct == null)
                    {
                        // If the product doesn't exist, return not found
                        return NotFound();
                    }

                    // Map the updated data from the createOrUpdate to the existing product
                    string userId = _userManager.GetUserId(User);
                    var user = await _userManager.FindByIdAsync(userId);
                    string fileName = "";
                    if (createOrUpdate.ImageUpload != null)
                        {
                            string fileExtension = Path.GetExtension(createOrUpdate.ImageUpload.FileName);
                            fileName = $"{user.Id}_{DateTime.Now.Ticks}_{createOrUpdate.ImageUpload.FileName}";
                            fileImage = fileName;
                            string filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await createOrUpdate.ImageUpload.CopyToAsync(fileStream);
                            }
                        if (!string.IsNullOrEmpty(existingProduct.Image))
                        {
                            string existingFilePath = Path.Combine(_environment.WebRootPath, "uploads", existingProduct.Image);
                            if (System.IO.File.Exists(existingFilePath))
                            {
                                System.IO.File.Delete(existingFilePath);
                            }
                        }
                    }
                    
                    // Set the UpdatedDate and UpdatedBy fields
                    _mapper.Map(createOrUpdate, existingProduct);
                    existingProduct.UpdatedDate = DateTime.UtcNow;
                    existingProduct.UpdatedBy = user.UserName;
                    existingProduct.Image = fileName;
                    // Clear existing product tags and update with the new ones
                    existingProduct.ProductTags.Clear();
                    if (createOrUpdate.ListSelectedTags != null)
                    {
                        foreach (var tagId in createOrUpdate.ListSelectedTags)
                        {
                            var productTag = new ProductTag
                            {
                                ProductID = existingProduct.ID,
                                TagID = tagId
                            };
                            existingProduct.ProductTags.Add(productTag);
                        }
                    }

                    await _productRepository.UpdateAsync(existingProduct);

                    await _productRepository.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                _logger.LogError(ex, "An error occurred while editing the product.");

                // Add an error message to ModelState
                ModelState.AddModelError(string.Empty, "An error occurred while editing the product.");
            }

            // If ModelState is not valid or an error occurred, return to the edit view with validation errors
            var categories = await _productCategoryRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();
            createOrUpdate.ListCategories = (List<ProductCategory>)categories;
            createOrUpdate.ListTags = (List<Tag>)tags;
            return RedirectToAction(nameof(Index));
        }


        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    // If id is not provided, return a bad request response
                    return BadRequest();
                }

                // Get the existing product from the repository
                var listOrderDetail = _orderDetailRepository.GetOrderDetalsByProductId(id.Value);
                if(listOrderDetail != null)
                {
                    foreach(var odt in listOrderDetail)
                    {
                        _orderDetailRepository.Delete(odt);
                    }
                }
                var existingProduct = await _productRepository.GetProductByIdAsync(id.Value);

                if (existingProduct == null)
                {
                    // If the product doesn't exist, return not found
                    return NotFound();
                }


                // Delete the product from the repository
                await _productRepository.DeleteAsync(existingProduct.ID);

                // Save changes to the repository
                await _productRepository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                _logger.LogError(ex, "An error occurred while deleting the product.");

                // Optionally, add an error message to TempData or ViewBag to display on the view
                TempData["ErrorMessage"] = "An error occurred while deleting the product.";

                return RedirectToAction(nameof(Index)); // Redirect to the index view even in case of error
            }
        }


        #region Handel cart 
        [AllowAnonymous]
        public async Task<ActionResult> AddToCart(int id, int quantity, decimal price, int categoryId, string image)
        {
            List<CartItem> myCart;

            // Check if session is null or not
            if (HttpContext.Session.GetString("myCart") == null)
            {
                // If it's null, create a new list of CartItem
                myCart = new List<CartItem>();
            }
            else
            {
                // If it's not null, deserialize the session data into a list of CartItem
                myCart = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("myCart"));
            }

            // Check if the product is already in the cart
            var existingItem = myCart.FirstOrDefault(item => item.ProductID == id);
            if (existingItem != null)
            {
                // If the product is already in the cart, update the quantity
                existingItem.Quantity += quantity;
            }
            else
            {
                // If not, add a new CartItem
                var product = await _productRepository.GetProductByIdAsync(id);
                var productCategory = await _productCategoryRepository.GetByIdAsync(categoryId);
                var cartItem = new CartItem
                {
                    ProductID = id,
                    Quantity = quantity,
                    Price = price,
                    Image = image,
                    CategoryID = categoryId,
                    ProductName = product.Name,
                    CategoryName = productCategory.Name,
                };

                myCart.Add(cartItem);
            }

            // Serialize and save the updated cart back to the session
            HttpContext.Session.SetString("myCart", JsonSerializer.Serialize(myCart));

            return RedirectToAction(nameof(Cart));
        }

        #endregion

        [AllowAnonymous]
        public async Task<ActionResult> Cart()
        {
            List<CartItem> myCart;

            // Check if session is null or not
            if (HttpContext.Session.GetString("myCart") == null)
            {
                // If it's null, create a new list of CartItem
                myCart = new List<CartItem>();
            }
            else
            {
                // If it's not null, deserialize the session data into a list of CartItem
                myCart = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("myCart"));
            }

            var model = new Cart();
            model.ListItems = myCart;

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult DeleteCartItem(int productId)
        {
            List<CartItem> myCart;
            myCart = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("myCart"));
            var x = myCart.FirstOrDefault(x => x.ProductID == productId);
            if (x != null)
            {
                myCart.Remove(x);
                HttpContext.Session.SetString("myCart", JsonSerializer.Serialize(myCart));
            }
            var countItemString = myCart.Count == 0 ? "No item in your cart"
                            : myCart.Count == 1 ? "1 item"
                            : $"{myCart.Count} items";
            var totalAmout = 0M;
            myCart.ForEach(x =>
            {
                totalAmout += x.Price * x.Quantity;
            });
            var response = new
            {
                countItemString = countItemString,
                total = totalAmout,
            };
            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(Cart cart)
        {
            List<CartItem> myCart = new List<CartItem>();
            if (HttpContext.Session.GetString("myCart") != null) myCart = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("myCart"));
            if(myCart.Count == 0)
            {
                return RedirectToAction(nameof(Cart));
            }
            var user = await _userManager.GetUserAsync(User);
            Order order = new Order()
            {
                CustomerName = cart.Name,
                CustomerAddress = cart.Address,
                CustomerMobile = cart.Phone,
                CustomerMessage = cart.Message,
                Status = false,
                PaymentMethod = "Online",
                PaymentStatus = "Successful",
                CustomerId = user.Id,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = user.UserName,
             };
             var orderSaved = await _orderRepository.AddAsync(order);
             foreach(var item in myCart)
            {
                var orderDetail = new OrderDetail
                {
                    ProductID = item.ProductID,
                    OrderID = orderSaved.ID,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                await _orderDetailRepository.AddAsync(orderDetail);
                Product currentP = await _productRepository.GetProductByIdAsync(item.ProductID);
                currentP.Quantity -= item.Quantity;
                await _productRepository.UpdateAsync(currentP);
            }
            HttpContext.Session.Remove("myCart");
            return RedirectToAction(nameof(Cart));    
        }
        [Authorize]
        public async Task<ActionResult> Order(string id)
        {
            
            var orders = _orderRepository.GetAllOrderByUserId(id);
            return View(orders);
        }

        [Authorize]
        public async Task<ActionResult> OrderDetail(int orderId)
        {
            
            var user = await _userManager.GetUserAsync(User);
            var order = _orderRepository.GetById(orderId);
            if(order == null)
            {
              return Redirect("/identity/errornotfound");
            }
            if(user.Id != order.CustomerId)
            {
                return Redirect("/identity/accessdenied");
            }
            var orderDetails = _orderRepository.GetAllOrderDetails(orderId);
            return View(orderDetails);
        }
      
    }
}
