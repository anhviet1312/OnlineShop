using Microsoft.AspNetCore.Mvc;
using ShopOnline.Interface;
using ShopOnline.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ShopOnline.Repository;

namespace ShopOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ProductCategoryController> _logger;
        private const int PRODUCT_CATEGORY_PER_PAGE = 1;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository, IMapper mapper, 
                                         UserManager<AppUser> userManager,
                                         ILogger<ProductCategoryController> logger)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: ProductCategoryController
        public async Task<ActionResult> Index(int ? pageNumber)
        {
            var viewProductCategory = new ViewProductCategoryModel();
            var productCategories = (List<ProductCategory>)await _productCategoryRepository.GetAllAsync();
            var totalPage = productCategories.Count > 0 ? (productCategories.Count - 1) / PRODUCT_CATEGORY_PER_PAGE + 1 : 0;
            int pageNumberValue = pageNumber ?? 1;
            var productCategoriesInPage = productCategories.Skip((pageNumberValue - 1) * PRODUCT_CATEGORY_PER_PAGE)
                               .Take(PRODUCT_CATEGORY_PER_PAGE).ToList();
            viewProductCategory.ProductCategories = productCategoriesInPage;
            viewProductCategory.TotalPages = totalPage;
            viewProductCategory.PageNumber = pageNumberValue;
            return View(viewProductCategory);
        }

        // GET: ProductCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductCategoryDto category)
        {
            if (ModelState.IsValid)
            {
                // Map the CreateProductCategoryDto to ProductCategory using AutoMapper
                var productCategory = _mapper.Map<ProductCategory>(category);
                

                string userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                productCategory.CreatedDate = DateTime.UtcNow; 
                productCategory.CreatedBy = user.UserName; 
                productCategory.UpdatedDate = DateTime.UtcNow; 
                productCategory.UpdatedBy = user.UserName; 
                await _productCategoryRepository.AddAsync(productCategory);

                await _productCategoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return to the create view with validation errors
            return View(category);
        }

        // GET: ProductCategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var x = await _productCategoryRepository.GetByIdAsync(id);
            return Json(x);
        }

        // POST: ProductCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int categoryId, CreateProductCategoryDto CreateOrUpdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the existing product category from the repository
                    var existingCategory = _productCategoryRepository.GetByIdAsync(categoryId).Result;

                    // Map the updated data from the CreateOrUpdate DTO to the existing category
                    _mapper.Map(CreateOrUpdate, existingCategory);

                    // Set the UpdatedDate and UpdatedBy fields
                    string userId = _userManager.GetUserId(User);
                    var user = await _userManager.FindByIdAsync(userId);
                    existingCategory.UpdatedDate = DateTime.UtcNow;
                    existingCategory.UpdatedBy = user.UserName;

                    // Update the category in the repository
                    await _productCategoryRepository.UpdateAsync(existingCategory);

                    // Save changes to the repository
                    await _productCategoryRepository.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }

                // If ModelState is not valid, return to the edit view with validation errors
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ProductCategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductCategoryController/Delete/5
        // POST: ProductCategoryController/Delete/5
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

                // Retrieve the product category by its ID
                var categoryToDelete = await _productCategoryRepository.GetByIdAsync(id.Value);

                if (categoryToDelete == null)
                {
                    // If the category does not exist, return a not found response
                    return NotFound();
                }

                // Delete the product category from the repository
                await _productCategoryRepository.DeleteAsync(id.Value);

                // Save changes to the repository
                await _productCategoryRepository.SaveAsync();

                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // If an error occurs, redirect to the index page
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
