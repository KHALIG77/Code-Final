using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Email {get; set;}
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set;}
        [Required]
        [MaxLength(100)]
        public string UserName { get; set;}
		[Required]
		[MinLength(3)]
		[MaxLength(100)]
		public string FullName {get; set;}
    }
}
