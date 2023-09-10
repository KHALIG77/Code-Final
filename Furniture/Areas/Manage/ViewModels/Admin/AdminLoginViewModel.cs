using System.ComponentModel.DataAnnotations;

namespace Furniture.Areas.Manage.ViewModels.Admin
{
    public class AdminLoginViewModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }    
    }
}
