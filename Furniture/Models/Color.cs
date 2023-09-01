using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Color
	{
		public int Id {get; set;}
		[Required]
		public string Name { get; set;}
	}
}
