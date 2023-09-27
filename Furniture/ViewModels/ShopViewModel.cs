using Furniture.Models;

namespace Furniture.ViewModels
{
	public class ShopViewModel
	{
		public List<Category> Categories { get; set; }
		public List<Brand> Brands { get; set; }
		
		public List<Tag> Tags { get; set; }	
		public PaginatedList<Product> PaginatedList { get; set;}
		public int AllPlantCount {get; set;}


	}
}
