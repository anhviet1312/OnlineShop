using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Interface;
using ShopOnline.Models;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        // GET: TagController
        public async Task<ActionResult> Index()
        {
            var tags = await _tagRepository.GetAllAsync();
            return View(tags);
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
                await _tagRepository.AddAsync(tag);
                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Tag tag)
        {
            try
            {
                // TODO: Implement the logic to update the tag in the repository
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                // If id is not provided, return a bad request response
                return BadRequest();
            }

            await _tagRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
