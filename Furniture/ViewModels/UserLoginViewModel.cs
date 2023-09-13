using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
		[MinLength(5)]
		[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
