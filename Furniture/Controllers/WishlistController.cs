using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Controllers
{
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
		public IActionResult AddToWishList(int id)
		{
			return Ok();
		}
	}
}
