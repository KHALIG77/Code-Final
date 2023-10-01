//using AspNetCore;
using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Furniture.Controllers
{
	[Authorize(Roles ="Member")]
	public class WishlistController : Controller
	{

		private readonly FurnutireContext _context;

		public WishlistController(FurnutireContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<WishlistItem> items =_context.WishlistItems.Include(x=>x.Product).ThenInclude(x=>x.Images).ToList();
			return View(items);
		}
		
		public IActionResult RemoveWishlist(int id)
		{
			List<WishlistItem> items = _context.WishlistItems.Include(x => x.Product).ThenInclude(x => x.Images).ToList();
			if (!_context.WishlistItems.Any(x=>x.ProductId==id))
			{
				return BadRequest();
			}
			else
			{
			  WishlistItem wish =_context.WishlistItems.Where(x => x.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault(x => x.ProductId == id);
			_context.WishlistItems.Remove(wish);
		    _context.SaveChanges();
				return PartialView("_WishlistPartialView", _context.WishlistItems.Where(x => x.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList());
			}


		}

	}
}
