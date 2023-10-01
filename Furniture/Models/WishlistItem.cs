using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class WishlistItem
	{
		public int Id {get; set;}
		[Required] 
		public string AppUserId {get; set;}
		[Required]
		public int ProductId {get; set; }
	
		[Required]
		public DateTime CreateAt {get; set;}
		public AppUser AppUser { get; set;}
		public Product Product { get; set;}

	}
}
