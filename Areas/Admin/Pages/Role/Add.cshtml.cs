﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopOnline.Areas.Admin.Pages.Role
{
    public class AddModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        public class InputModel
        {
            public string? ID { set; get; }

            [Required(ErrorMessage = "Role name is required")]
            [Display(Name = "Role Name")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }

        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool IsUpdate { set; get; }

        // Không cho truy cập trang mặc định mà không có handler
        public IActionResult OnGet() => NotFound("Not Found");
        public IActionResult OnPost() => NotFound("Not Found");


        public IActionResult OnPostStartNewRole()
        {
            StatusMessage = "Fill form for create new role";
            IsUpdate = false;
            ModelState.Clear();
            return Page();
        }

        // Truy vấn lấy thông tin Role cần cập nhật
        public async Task<IActionResult> OnPostStartUpdate()
        {
            StatusMessage = null;
            IsUpdate = true;
            if (Input.ID == null)
            {
                StatusMessage = "Error: Cannot find information of role";
                return Page();
            }
            var result = await _roleManager.FindByIdAsync(Input.ID);
            if (result != null)
            {
                Input.Name = result.Name;
                ViewData["Title"] = "Update role : " + Input.Name;
                ModelState.Clear();
            }
            else
            {
                StatusMessage = "Error: Cannot find information of role with ID = " + Input.ID;
            }

            return Page();
        }

        // Cập nhật hoặc thêm mới tùy thuộc vào IsUpdate
        public async Task<IActionResult> OnPostAddOrUpdate()
        {

            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return Page();
            }

            if (IsUpdate)
            {
                // CẬP NHẬT
                if (Input.ID == null)
                {
                    ModelState.Clear();
                    StatusMessage = "Error: Cannot find role info";
                    return Page();
                }
                var result = await _roleManager.FindByIdAsync(Input.ID);
                if (result != null)
                {
                    result.Name = Input.Name;
                    // Cập nhật tên Role
                    var roleUpdateRs = await _roleManager.UpdateAsync(result);
                    if (roleUpdateRs.Succeeded)
                    {
                        StatusMessage = "Update role succesfully";
                    }
                    else
                    {
                        StatusMessage = "Error: ";
                        foreach (var er in roleUpdateRs.Errors)
                        {
                            StatusMessage += er.Description;
                        }
                    }
                }
                else
                {
                    StatusMessage = "Error: Cannot find role";
                }

            }
            else
            {
                // TẠO MỚI
                var newRole = new IdentityRole(Input.Name);
                // Thực hiện tạo Role mới
                var rsNewRole = await _roleManager.CreateAsync(newRole);
                if (rsNewRole.Succeeded)
                {
                    StatusMessage = $"Create new role successfully: {newRole.Name}";
                    return RedirectToPage("./Index");
                }
                else
                {
                    StatusMessage = "Error: ";
                    foreach (var er in rsNewRole.Errors)
                    {
                        StatusMessage += er.Description;
                    }
                }
            }

            return Page();

        }
    }
}