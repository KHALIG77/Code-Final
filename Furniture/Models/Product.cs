using Furniture.Attiributes.ValidationAttiribute;
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
		public bool IsBest {get; set; }
		public bool IsFeatured {get; set; }
		public bool IsStock { get; set; }
		public int? BrandId { get; set; }	
		public Brand Brand { get; set; }	
		
		public Category Category { get; set; }
		public List<ProductTag> Tags { get; set; } = new List<ProductTag>();
		public byte Rate {get; set; }
		public List<Comment> Comments { get; set; } = new List<Comment>();
		public List<ProductImage> Images { get; set; }=new List<ProductImage>();
		public List<ProductSize> Sizes { get; set; } = new List<ProductSize>();
		public List<ProductColor> Colors { get; set; } = new List<ProductColor>();
		[Required]
		public int MaterialId { get; set; }
		public Material Material { get; set; }
		[NotMapped]
		public List<int> TagIds {get; set; } = new List<int>();
    
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg", "image/jpg","image/avif")]
        [FileSize(1000000)]
        public IFormFile? MainImage {get; set;}
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg", "image/jpg", "image/avif")]
        [FileSize(1000000)]
        public IFormFile? HoverImage {get; set;}
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg", "image/jpg", "image/avif")]
        [FileSize(1000000)]
        public List<IFormFile>? AllImages { get; set;} = new List<IFormFile>();
		[NotMapped]
		public List<int> ColorIds {get; set;} = new List<int>();
		[NotMapped]
		public List<int> SizeIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> ImageIds { get; set; } = new List<int>();
		[Required]
		[MaxLength(1000)]
		public string Detail {get; set;}


	}
}
