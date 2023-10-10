using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

		public IActionResult Index(int? materialid,int? brandid,int? categoryid,int? minprice,int? maxprice,List<int> tags = null, int page=1,string size = null,string sort=null)
		{
			ShopViewModel vm = new ShopViewModel
			{
				Categories = _context.Categories.Include(x => x.Products).ToList(),
				Tags = _context.Tags.ToList(),
				Brands = _context.Brands.ToList(),
				Materials = _context.Materials.Include(x=>x.Products).ToList()
			};
			var query = _context.Products.Include(x=>x.Images).Include(x=>x.Comments).Include(x=>x.Material).Include(x=>x.Category).Include(x=>x.Tags).AsQueryable();
            if (sort != null)
            {
                switch (sort)
                {
                    case "AToZ":
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "ZToA":
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    case "LowToHigh":
                        query = query.OrderBy(x => x.SalePrice);
                        break;
                    case "HighToLow":
                        query = query.OrderByDescending(x => x.SalePrice);
                        break;
                }
            }
            if (categoryid != null)
			{
				query = query.Where(x => x.CategoryId == categoryid);
			}
			if (materialid != null)
			{
				query = query.Where(x => x.MaterialId == materialid);
			}
			if (brandid != null)
			{
				query = query.Where(x => x.BrandId == brandid);
			}

			if (minprice != null && maxprice!=null)
			{
				query = query.Where(x => x.SalePrice >= minprice && x.SalePrice <= maxprice);
			}
			if (tags.Count > 0)
			{
				query = query.Where(product => product.Tags.Any(tag => tags.Contains(tag.TagId)));
			}
            ViewBag.SortList = new List<SelectListItem>
            {
                new SelectListItem{ Value="AToZ",Text="Sort By Name (A-Z)",Selected=sort=="AToZ"},
                new SelectListItem{ Value="ZToA",Text="Sort By Name (Z-A)",Selected=sort=="ZToA"},
                new SelectListItem{ Value="LowToHigh",Text="Sort By Price(LOW-HIGH)",Selected=sort=="LowToHigh"},
                new SelectListItem{ Value="HighToLow",Text="Sort By Price (HIGH-LOW)",Selected=sort=="HighToLow"}


            };
            ViewBag.CategoryId = categoryid;
			ViewBag.MaterialId = materialid;
			ViewBag.BrandId = brandid;	
			ViewBag.MinLimit  =_context.Products.Any() ?(int)_context.Products.Min(x=>x.SalePrice) :0;
			ViewBag.MaxLimit = _context.Products.Any() ?(decimal) _context.Products.Max(x => x.SalePrice) : 0;
			ViewBag.MinPrice = minprice??(decimal?) _context.Products.Min(x => (decimal?)x.SalePrice);
			ViewBag.MaxPrice = maxprice??(decimal?) _context.Products.Max(x => (decimal?)x.SalePrice);
			ViewBag.Tags=tags;
			ViewBag.Sort=sort;

			query.ToList();
			vm.PaginatedList = PaginatedList<Product>.Create(query, page, 6);

			return View(vm);
		}
	}
}
