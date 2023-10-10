﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string FullName { get; set; }
        public bool IsStaff { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string Address { get; set; }
        public string Role {get; set; }
    }
}
