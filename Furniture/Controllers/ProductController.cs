using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
			Product product = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Comments).Include(x => x.Images).Include(x => x.Material).Include(x => x.Sizes).ThenInclude(s => s.Size).Include(x => x.Colors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);

			if (product == null)
			{
				return View("Error");
			}
			ProductDetailViewModel vm = new ProductDetailViewModel
			{
				Product = product,
				RelatedProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).ThenInclude(t => t.Tag).Include(x => x.Comments).Include(x => x.Images).Where(x => x.CategoryId == product.CategoryId).ToList(),
			};

			return View(vm);
		}
		public IActionResult AddToBasket(int id)
		{
			List<BasketCkItemViewModel> basketItems = new List<BasketCkItemViewModel>();
			BasketCkItemViewModel item;
			var basketStr = Request.Cookies["Basket"];
			if (basketStr != null)
			{
				basketItems = JsonConvert.DeserializeObject<List<BasketCkItemViewModel>>(basketStr);
				item = basketItems.FirstOrDefault(x => x.ProductId == id);
				if (item != null)
				{
					item.Count++;
				}
				else
				{
					item = new BasketCkItemViewModel()
					{
						ProductId = id,
						Count = 1,
					};
					basketItems.Add(item);
				}
			}
			else
			{
				item = new BasketCkItemViewModel
				{
					ProductId = id,
					Count = 1,
				};
				basketItems.Add(item);
			}
			HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basketItems));
			return PartialView("_BasketPartialView", GenerateBasketVM(basketItems));

		}
		private BasketViewModel GenerateBasketVM(List<BasketCkItemViewModel> cookieItems)
		{
			BasketViewModel bv = new BasketViewModel();
			foreach (var ci in cookieItems)
			{
				BasketItemViewModel bi = new BasketItemViewModel
				{
					Count = (int)ci.Count,
					Product = _context.Products.Include(x => x.Images.Where(x => x.Status == Enums.ImageStatus.Main)).FirstOrDefault(x => x.Id == ci.ProductId),

				};
				bv.Items.Add(bi);
				bv.TotalCount = (byte)cookieItems.Count;
				bv.TotalPrice += (bi.Product.DiscountPercent > 0 ? ((bi.Product.SalePrice * (100 - bi.Product.DiscountPercent) / 100) * bi.Count) : bi.Product.SalePrice * bi.Count);
			}
			return bv;

		}
	}
}
