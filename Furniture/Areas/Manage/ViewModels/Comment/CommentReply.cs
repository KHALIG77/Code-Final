using System.ComponentModel.DataAnnotations;

namespace Furniture.Areas.Manage.ViewModels.Comment
{
    public class CommentReply
    {  
        public int CommentId {get; set;}
        public string Comment {get; set;}
        
        [MaxLength(400)]
        public string Reply {get; set;}
    }
}
