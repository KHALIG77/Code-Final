using Furniture.Areas.Manage.ViewModels.Comment;
using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Areas.Manage.Controllers
{
	[Area("manage")]
	public class CommentController : Controller
	{
		private readonly FurnutireContext _context;

		public CommentController(FurnutireContext context)
		{
		    _context = context;
		}
		public IActionResult Index(string search=null,int page=1)
		{
			var query = _context.Comments.Include(x=>x.AppUser).AsQueryable();
			if (search!=null)
			{
				query=query.Where(x=>x.AppUser.UserName.Contains(search));

			}
			ViewBag.Search = search;

			return View(PaginatedList<Comment>.Create(query, page, 2));
		}
		public IActionResult Detail(int id)
		{
			ViewBag.CommentId =id;
            Comment comment = _context.Comments.FirstOrDefault(x => x.Id == id);

            if (comment==null)
			return View("Error");
			CommentReply commentReply = new CommentReply
			{
				CommentId = id,
				Comment = comment.CommentText,
				Reply = comment.ReplyComment
			};
           
			return View(commentReply);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public IActionResult Detail(CommentReply commentReply)
		{
            ViewBag.CommentId = commentReply.CommentId;

            if (!ModelState.IsValid)
			{
				ModelState.AddModelError("Reply", "Please write correctly");
				return View();
			}
			var comment = _context.Comments.FirstOrDefault(x => x.Id == commentReply.CommentId);

			if (comment==null)
			{
				return View("Error");
			}
			comment.ReplyComment = commentReply.Reply;
			comment.ReplyTime=DateTime.Now.AddHours(4);

			_context.SaveChanges();


			return RedirectToAction("index");

				

		}
	}
}
