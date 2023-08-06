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

namespace ShopOnline.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(ApplicationDbContext context, IProductRepository productRepository, 
                                             ILogger<HomeController> logger, IProductCategoryRepository productCategoryRepository,
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
            var products = await _productRepository.GetAllAsync();
            return View(products);
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

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
