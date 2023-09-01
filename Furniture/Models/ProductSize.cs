using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class ProductSize
	{
		public int Id { get; set; }
		[Required]
		public int SizeId {get; set; }
		[Required]
	    public int ProductId {get; set; }
		public Size Size { get; set; }
		public Product Product { get; set; }
	
	}
}
