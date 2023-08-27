using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Brand
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Image {get; set; }
	}
}
