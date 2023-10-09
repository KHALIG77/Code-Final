using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Areas.Manage.ViewModels.Staff
{
    public class StaffCreateViewModel
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }

        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(20)]
        public string NewPassword { get; set; }

        [Required]
        public string Role{get; set; }
        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
    }
}
