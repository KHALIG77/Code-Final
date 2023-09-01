using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class ProductColor
	{
		public int Id { get; set; }
		[Required]
		public int ProductId {get; set; }
		[Required]
		public int ColorId {get; set; }
		public Product Product { get; set; }
		public Color Color { get; set; }

	}
}
