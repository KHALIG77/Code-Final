using System.ComponentModel.DataAnnotations;

namespace Furniture.ViewModels
{
	public class CommentForm
	{
		public int ProductId {get; set;}
		
		[Required]
	    public int Rate {get; set;}
		[Required]
		[MaxLength(400)]
	   public string Text {get; set;}

	}
}
