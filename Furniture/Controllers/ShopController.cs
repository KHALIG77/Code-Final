using Furniture.DAL;
using Furniture.Models;
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

		public IActionResult Index(int? materialid,int? brandid,int? categoryid,int? minprice,int? maxprice,List<int> tags = null, int page=1,string size = null)
		{
			ShopViewModel vm = new ShopViewModel
			{
				Categories = _context.Categories.Include(x => x.Products).ToList(),
				Tags = _context.Tags.ToList(),
				Brands = _context.Brands.ToList(),
				Materials = _context.Materials.Include(x=>x.Products).ToList()
			};
			var query = _context.Products.Include(x=>x.Images).Include(x=>x.Comments).Include(x=>x.Material).Include(x=>x.Category).Include(x=>x.Tags).AsQueryable();
			if (categoryid != null)
			{
				query = query.Where(x => x.CategoryId == categoryid);
			}
			if (brandid != null)
			{
				query = query.Where(x => x.BrandId == brandid);
			}

			if (minprice != null && maxprice!=null)
			{
				query = query.Where(x => x.SalePrice > minprice && x.SalePrice < maxprice);
			}
			if (tags.Count > 0)
			{
				query = query.Where(product => product.Tags.Any(tag => tags.Contains(tag.TagId)));
			}
			ViewBag.CategoryId = categoryid;
			ViewBag.MaterialId = materialid;
			ViewBag.BrandId = brandid;	
			ViewBag.MinLimit  =_context.Products.Any() ? _context.Products.Min(x=>x.SalePrice) :0;
			ViewBag.MaxLimit = _context.Products.Any() ? _context.Products.Max(x => x.SalePrice) : 0;
			ViewBag.MinPrice = minprice;
			ViewBag.MaxPrice = maxprice;
			ViewBag.Tags=tags;

			query.ToList();
			vm.PaginatedList = PaginatedList<Product>.Create(query, page, 1);

			return View(vm);
		}
	}
}
