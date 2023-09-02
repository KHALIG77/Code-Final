using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Tag
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(60)]
		[MinLength(3)]
		public string Name { get; set; }	
		public List<ProductTag> Products { get; set; }
	}
}
