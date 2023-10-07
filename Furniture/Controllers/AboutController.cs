using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Controllers
{
	public class AboutController : Controller
	{
		private readonly FurnutireContext _context;

		public AboutController(FurnutireContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			AboutViewModel vm = new AboutViewModel

			{   Sliders = _context.Sliders.OrderByDescending(x => x.Order).Where(x => x.ForAbout == true).Take(2).ToList(),
				Brand = _context.Brands.Take(4).ToList(),
				Features = _context.Features.ToList(),
				Comments = _context.Comments.Include(x=>x.AppUser).Where(x => x.IsShow == true).Take(4).ToList(),
			};

			return View(vm);
		}
	}
}
