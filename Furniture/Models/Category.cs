using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Category
	{
		public int Id {get; set;}
		[Required]
		[MaxLength(60)]
		public string Name { get; set;}	

	}
}
