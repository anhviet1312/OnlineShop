using Microsoft.AspNetCore.Mvc;
using ShopOnline.Interface;
using ShopOnline.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models.ViewModels;

namespace ShopOnline.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ProductCategoryController> _logger;

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
        public async Task<ActionResult> Index()
        {
            var viewProductCategory = new ViewProductCategoryModel();
            viewProductCategory.ProductCategories = (List<ProductCategory>) await _productCategoryRepository.GetAllAsync();
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

                return RedirectToAction("Index", "Home");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                // If id is not provided, return a bad request response
                return BadRequest();
            }

            await _productCategoryRepository.DeleteAsync(id.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
