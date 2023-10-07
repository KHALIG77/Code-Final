using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Setting
	{
		[Required]
		[MaxLength(100)]
		public string Key {get; set;}
		public string Value { get; set;}
	}
}
