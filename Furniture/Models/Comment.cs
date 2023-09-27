using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Comment
	{
		public int Id { get; set; }
		
		public string AppUserId {get; set;}
		[Required]
		public int ProductId {get; set;}
		[Required]
		[Range(0,5)]
		public byte Rate { get; set;}
		[Required]
		[MaxLength(400)]
		public string CommentText { get; set;}
		[MaxLength(400)]
		public string ReplyComment {get; set;}
		public DateTime CreatedAt {get; set;}
		public DateTime ReplyTime {get; set;}
		public AppUser AppUser { get; set;}
		public Product Product { get; set;}
		public bool IsShow {get; set;}	


	}
}
