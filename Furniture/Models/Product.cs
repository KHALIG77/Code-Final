using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furniture.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[Required]
		[MaxLength(200)]
		public string Description { get; set; }
		[Required]
		public int CategoryId {get; set; }
		[Required]
		[Column(TypeName="money")]
		public decimal SalePrice { get; set; }
		[Required]
		[Column(TypeName = "money")]
		public decimal CostPrice {get; set; }
		
		[Column(TypeName = "money")]
		public decimal DiscountPercent {get; set; }
		public bool IsNew {get; set; }
		public bool IsFeatured {get; set; }
		public bool IsStock {get; set; }
		
		public Category Category { get; set; }
		public List<ProductTag> Tags { get; set; } = new List<ProductTag>();
		public byte Rate {get; set; }
		public List<Comment> Comments { get; set; } = new List<Comment>();
		public List<ProductImage> Images { get; set; }=new List<ProductImage>();


	}
}
