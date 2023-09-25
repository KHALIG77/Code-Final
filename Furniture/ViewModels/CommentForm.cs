using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
	public class CommentForm
	{
		[Required]

	    public int Rate {get; set;}
		[Required]
		[MaxLength(400)]
	   public string Text {get; set;}

	}
}
