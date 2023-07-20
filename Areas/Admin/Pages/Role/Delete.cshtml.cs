using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Areas.Admin.Pages.Role
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public class InputModel
        {
            [Required]
            public string ID { set; get; }
            public string? Name { set; get; }

        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool isConfirmed { set; get; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        // Delete chỉ được cho phép truy cập khi nhận post
        public IActionResult OnGet() => NotFound("Not Found");

        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                //var s = "";
                //foreach (var key in ModelState.Keys)
                //{
                //    var state = ModelState[key];
                //    if (state.Errors.Count > 0)
                //    {
                //        foreach (var error in state.Errors)
                //        {
                //            var errorMessage = error.ErrorMessage;
                //            s += errorMessage;
                //            // Do something with the error message, like logging or displaying it in the view.
                //        }
                //    }
                //}
                return NotFound("Error occured when delete");
            }

            var role = await _roleManager.FindByIdAsync(Input.ID);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            ModelState.Clear();

            if (isConfirmed)
            {
                //Xóa
                await _roleManager.DeleteAsync(role);
                StatusMessage = "Deleted role: " + role.Name;

                return RedirectToPage("Index");
            }
            else
            {
                Input.Name = role.Name;
                isConfirmed = true;

            }

            return Page();
        }
    }
}
