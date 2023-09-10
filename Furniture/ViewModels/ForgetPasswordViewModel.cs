using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
