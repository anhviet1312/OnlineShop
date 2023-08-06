using Microsoft.AspNetCore.Mvc;
using ShopOnline.Interface;
using ShopOnline.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShopOnline.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository, IMapper mapper, 
                                         UserManager<AppUser> userManager)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: ProductCategoryController
        public async Task<ActionResult> Index()
        {
            var productCategories = await _productCategoryRepository.GetAllAsync();
            return View(productCategories);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductCategory productCategory)
        {
            try
            {
                // TODO: Implement the logic to update the product category in the repository
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
