using Furniture.Models;

namespace Furniture.ViewModels
{
	public class OrderViewModel
	{
		public List<CheckoutItem> Items { get; set; }	
        public OrderFormViewModel Form { get; set; }
		public decimal TotalPrice {get; set; }


	}
}
