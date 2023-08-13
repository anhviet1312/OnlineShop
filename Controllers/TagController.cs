using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Interface;
using ShopOnline.Models;
using ShopOnline.Models.ViewModels;
using ShopOnline.Repository;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<TagController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public TagController(ITagRepository tagRepository, ILogger<TagController> logger, UserManager<AppUser> userManager)
        {
            _tagRepository = tagRepository;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: TagController
        public async Task<ActionResult> Index()
        {
            var viewTagModel = new ViewTagModel();

            viewTagModel.Tags = (List<Tag>) await _tagRepository.GetAllAsync();
            return View(viewTagModel);
        }

        // GET: TagController/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);
                tag.UpdatedDate = DateTime.UtcNow;
                tag.UpdatedBy = user.UserName;
                tag.CreatedDate = DateTime.UtcNow;
                tag.CreatedBy = user.UserName;
                await _tagRepository.AddAsync(tag);
                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }

        // GET: TagController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);

            return Json(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Tag createOrUpdate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userId = _userManager.GetUserId(User);
                    var user = await _userManager.FindByIdAsync(userId);
                    var currentTag = await _tagRepository.GetByIdAsync(createOrUpdate.ID);
                    if (currentTag != null)
                    {
                        currentTag.Name = createOrUpdate.Name;
                        currentTag.Type = createOrUpdate.Type;
                        currentTag.UpdatedDate = DateTime.UtcNow;
                        currentTag.UpdatedBy = user.UserName;
                        _tagRepository.SaveAsync();
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while editing the tag."); // Log the error
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: TagController/Delete/5
        public ActionResult DeleteGet(string id)
        {
            return View();
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    // If id is not provided, return a bad request response
                    return BadRequest();
                }

                // Get the existing product from the repository
                var tag = await _tagRepository.GetByIdAsync(id);

                if (tag == null)
                {
                    // If the product doesn't exist, return not found
                    return NotFound();
                }

                // Delete the product from the repository
                await _tagRepository.DeleteAsync(tag.ID);

                // Save changes to the repository
                await _tagRepository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                _logger.LogError(ex, "An error occurred while deleting the tag.");

                // Optionally, add an error message to TempData or ViewBag to display on the view
                TempData["ErrorMessage"] = "An error occurred while deleting the tag.";

                return RedirectToAction(nameof(Index)); // Redirect to the index view even in case of error
            }
        }
    
    }
}
