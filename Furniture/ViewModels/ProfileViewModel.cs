using Furniture.Models;

namespace Furniture.ViewModels
{
	public class ProfileViewModel
	{
		public ProfileEditViewModel Edit { get; set; }
		public List<Order> Orders { get; set; }
	}
}
