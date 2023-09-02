using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Controllers
{
	public class ProductController : Controller
	{
		private readonly FurnutireContext _context;
        public ProductController(FurnutireContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Detail(int id)
		{
			Product product = _context.Products.Include(x=>x.Category).Include(x=>x.Tags).Include(x=>x.Comments).Include(x=>x.Images).FirstOrDefault(x=>x.Id==id);

			if (product==null)
			{
				return View("Error");
			}
			ProductDetailViewModel vm = new ProductDetailViewModel
			{
				Product=product,
				RelatedProducts=_context.Products.Include(x => x.Category).Include(x => x.Tags).ThenInclude(t=>t.Tag).Include(x => x.Comments).Include(x => x.Images).Where(x=>x.CategoryId==product.CategoryId).ToList(),
			};

			return View(vm);
		}
	}
}
