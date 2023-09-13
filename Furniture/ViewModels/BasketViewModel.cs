namespace Furniture.ViewModels
{
	public class BasketViewModel
	{
		public List<BasketItemViewModel> Items { get; set; }
		public decimal TotalPrice {get; set;}
		public byte TotalCount {get; set;}
	}
}
