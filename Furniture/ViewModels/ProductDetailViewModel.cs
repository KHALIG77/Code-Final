using Furniture.Models;

namespace Furniture.ViewModels
{
	public class ProductDetailViewModel
	{
		public Product Product { get; set; }
		public List<Product> RelatedProducts { get; set; }
		
	}
}
