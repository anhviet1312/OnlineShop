﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ShopOnline.Models;

namespace ShopOnline.Areas.Identity.Pages
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;

		// Các dịch vụ được Inject vào: UserManger, SignInManager, ILogger, IEmailSender
		public RegisterModel(
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
		}

		// InputModel được binding khi Form Post tới

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "{0} has length in range [{2};{1}].", MinimumLength = 3)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "Confirm password not match")]
			public string ConfirmPassword { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "{0} has length in range [{2};{1}].", MinimumLength = 3)]
			[DataType(DataType.Text)]
			[Display(Name = "Username")]
			public string UserName { set; get; }
		}

		public async Task OnGetAsync(string? returnURL = null)
		{
			//ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string? returnURL = null)
		{
			returnURL = returnURL ?? Url.Content("~/");
			foreach (var modelStateEntry in ModelState.Values)
			{
				foreach (var error in modelStateEntry.Errors)
				{
					_logger.LogError(error.ErrorMessage);
				}
			}
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				var user = new AppUser { UserName = Input.UserName, Email = Input.Email };
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("Vừa tạo mới tài khoản thành công.");

					// phát sinh token
	
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

					// callbackUrl = /Account/ConfirmEmail?userId=useridxx&code=codexxxx
					// Link trong email Page: /Acount/ConfirmEmail
					var callbackUrl = Url.Page(
						"/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = user.Id, code = code, returnURL = returnURL },
						protocol: Request.Scheme);

					// Gửi email    
					await _emailSender.SendEmailAsync(Input.Email, "Confirma account's email",
						$"Confirm your email by clicking <a href='{callbackUrl}'>Click here</a>.");

					if (_userManager.Options.SignIn.RequireConfirmedEmail)
					{
						// Nếu cấu hình phải xác thực email mới được đăng nhập thì chuyển hướng đến trang
						// RegisterConfirmation - chỉ để hiện thông báo cho biết người dùng cần mở email xác nhận
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnURL = returnURL });
					}
					else
					{
						// Không cần xác thực
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnURL);
					}
				}
				// add lỗi vào model state
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			else
			{
				//foreach (var modelStateEntry in ModelState.Values)
				//{
				//	foreach (var error in modelStateEntry.Errors)
				//	{
				//		_logger.LogError(error.ErrorMessage);
				//	}
				//}
			}

			return Page();
		}
	}
}
