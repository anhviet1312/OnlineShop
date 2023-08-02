using Microsoft.AspNetCore.Mvc;
using ShopOnline.Interface;
using ShopOnline.Models;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
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
        public async Task<ActionResult> Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                await _productCategoryRepository.AddAsync(productCategory);
                return RedirectToAction(nameof(Index));
            }

            return View(productCategory);
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
