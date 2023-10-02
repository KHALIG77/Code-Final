using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Subscribe
	{
		public int Id {get; set;}
		[Required]
		[MaxLength(120)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email {get; set;}
	}
}
