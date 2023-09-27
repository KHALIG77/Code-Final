using Furniture.Models;

namespace Furniture.ViewModels
{
	public class ProductDetailViewModel
	{
		public Comment CommentForm { get; set; }
		public Product Product { get; set; }
		public List<Product> RelatedProducts { get; set; }
		public bool AccessComment {get; set;}
		
	}
}
