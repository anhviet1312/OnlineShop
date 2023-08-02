using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;
using ShopOnline.Repository;

namespace ShopOnline.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;
        public ProductController(ApplicationDbContext context, IProductRepository productRepository, ILogger<HomeController> logger)
        {
            _context = context;
            _productRepository = productRepository;
            _logger = logger;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
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
            return View(product);
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
