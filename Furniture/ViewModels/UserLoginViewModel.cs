using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
