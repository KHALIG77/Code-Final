using Furniture.Models;

namespace Furniture.ViewModels
{
	public class AboutViewModel
	{  
		public List<Slider> Sliders {get; set;}	=new List<Slider>();
		public List<Brand> Brand { get; set; }=new List<Brand>();
		public List<Feature> Features { get; set; } = new List<Feature>();
		public List<Comment> Comments { get; set; }=new List<Comment>();
		

	}
}
