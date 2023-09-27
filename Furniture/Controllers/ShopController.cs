using Furniture.DAL;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Controllers
{
	public class ShopController : Controller
	{
		private readonly FurnutireContext _context;

		public ShopController(FurnutireContext context)
		{
			_context = context;
		}

		public IActionResult Index(int? categoryid,int? minprice,int? maxprice,int page=1,string size = null)
		{
			ShopViewModel vm = new ShopViewModel
			{
				Categories=_context.Categories.Include(x=>x.Products).ToList(),
				Tags=_context.Tags.ToList(),
				Brands=_context.Brands.ToList(),
			};
			return View(vm);
		}
	}
}
