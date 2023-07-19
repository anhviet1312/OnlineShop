﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Pages.Role
{
    public class UserModel : PageModel
    {
        const int USER_PER_PAGE = 2;
        private readonly RoleManager<IdentityRole> _roleManage;
        private readonly UserManager<AppUser> _userManager;

        public UserModel(RoleManager<IdentityRole> roleManage, UserManager<AppUser> userManager)
        {
            _roleManage = roleManage;
            _userManager = userManager;
        }
        public class UserInList : AppUser
        {
            // Liệt kê các Role của User ví dụ: "Admin,Editor" ...
            public string ListRoles { set; get; }
        }
        public List<UserInList> users;
        public int TotalPages { set; get; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int pageNumber { set; get; }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            await _userManager.AddToRoleAsync(user, "Editor");
            if (pageNumber == 0)
                pageNumber = 1;

            var listUsers = from u in _userManager.Users
                             orderby u.UserName
                             select new UserInList()
                             {
                                 Id = u.Id,
                                 UserName = u.UserName
                             };
            int totalUsers = await listUsers.CountAsync();

            TotalPages = (int)Math.Ceiling((double)totalUsers / USER_PER_PAGE);

            users = await listUsers.Skip(USER_PER_PAGE * (pageNumber - 1)).Take(USER_PER_PAGE).ToListAsync();

            //users.ForEach(async (u) =>
            //{
            //    var roles = await _userManager.GetRolesAsync(u);
            //    u.ListRoles = string.Join(",", roles.ToList());
            //});
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                u.ListRoles = string.Join(",", roles.ToList());
            }

            return Page();
        }

        public IActionResult OnPost() => NotFound("Cannot use post");

        
    }
}
