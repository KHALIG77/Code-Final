using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public string Address { get; set; }
    }
}
