using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;
using ShopOnline.Models.CreateModels;
using ShopOnline.Repository;
using Microsoft.AspNetCore.Identity;
using ShopOnline.Models.ViewModels;

namespace ShopOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        // GET: ProductController
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(ApplicationDbContext context, IProductRepository productRepository, 
                                             ILogger<ProductController> logger, IProductCategoryRepository productCategoryRepository,
                                             ITagRepository tagRepository,
                                             IMapper mapper,
                                             UserManager<AppUser> userManager)
        {
            _context = context;
            _productRepository = productRepository;
            _logger = logger;
            _productCategoryRepository = productCategoryRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            var productViewModel = new ViewProductModel();
            var categories = await _productCategoryRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();
            productViewModel.Products = await _productRepository.GetAllAsync();
            productViewModel.CreateOrUpdate = new CreateProductDto
            {
                ListCategories = (List<ProductCategory>)categories,
                ListTags = (List<Tag>)tags
            };
            return View(productViewModel);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // GET: ProductController/Create
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = _mapper.Map<Product>(productDto);
                    string userId = _userManager.GetUserId(User);
                    var user = await _userManager.FindByIdAsync(userId);
                    product.CreatedDate = DateTime.UtcNow;
                    product.CreatedBy = user.UserName;
                    product.UpdatedDate = DateTime.UtcNow;
                    product.UpdatedBy = user.UserName;
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
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                _logger.LogError(ex, "An error occurred while creating the product.");

                // Add an error message to ModelState
                ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
            }
            return View(productDto);
        }
        [Authorize(Roles = "Admin")]
        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return Json(product);
        }
        [Authorize(Roles = "Admin")]
        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int productid, CreateProductDto createOrUpdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the existing product from the repository
                    var existingProduct = await _productRepository.GetProductByIdWithListTagsAsync(productid);

                    if (existingProduct == null)
                    {
                        // If the product doesn't exist, return not found
                        return NotFound();
                    }

                    // Map the updated data from the createOrUpdate to the existing product
                    _mapper.Map(createOrUpdate, existingProduct);

                    // Set the UpdatedDate and UpdatedBy fields
                    string userId = _userManager.GetUserId(User);
                    var user = await _userManager.FindByIdAsync(userId);
                    existingProduct.UpdatedDate = DateTime.UtcNow;
                    existingProduct.UpdatedBy = user.UserName;

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

                    // Update the product in the repository
                    await _productRepository.UpdateAsync(existingProduct);

                    // Save changes to the repository
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

        [Authorize(Roles = "Admin")]
        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

    }
}
