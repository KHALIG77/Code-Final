using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Feature
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Title { get; set; }
		[Required]
		[MaxLength(70)]
		public string Description { get; set; }
		[Required]
		public string ImageSvg {get; set; }
	

	}
}
