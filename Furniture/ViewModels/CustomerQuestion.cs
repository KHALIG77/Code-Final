using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
	public class CustomerQuestion
	{
		[Required]
		[MaxLength(100)]
		public string Name {get; set;}
		[Required]
		[MaxLength(120)]
		public string Email {get; set;}
		[Required]
		[MaxLength(300)]
		public string Text {get; set;}
	}
}
