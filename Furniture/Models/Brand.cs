using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furniture.Models
{
	public class Brand
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Image {get; set; }
		[NotMapped]
		public IFormFile ImageUrl { get; set; }
		[Required]
		public string Name {get; set; }
		List<Product> Products { get; set; }
	}
}
