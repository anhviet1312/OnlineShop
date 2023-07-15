﻿using Microsoft.AspNetCore.Identity;

namespace ShopOnline.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime Dob { get; set; }
    }
}
