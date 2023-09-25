using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class InstagramPhoto
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Image { get; set; }
		[Required]
		public string Url {get; set; }
	}
}
