using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopOnline.Models;
using ShopOnline.Views.Shared.Components.MessagePage;

namespace ShopOnline.Areas.Identity.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            if (!_signInManager.IsSignedIn(User)) return RedirectToPage("/Index");

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logout");


            return ViewComponent(MessagePage.COMPONENTNAME,
                new MessagePage.Message()
                {
                    title = "Đã đăng xuất",
                    htmlcontent = "Đăng xuất thành công",
                    urlredirect = returnUrl ?? Url.Page("/Index")
                }
            );
        }
    }
}
