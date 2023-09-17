using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

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
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var basketItem =_context.BasketItems.FirstOrDefault(x=>x.ProductId==id && x.AppUserId==userId);
				if (basketItem != null)
				{
					basketItem.Count++;
				}
				else
				{
					basketItem = new BasketItem
					{
						AppUserId=userId,
						Count=1,
						ProductId=id,
					};
					_context.BasketItems.Add(basketItem);
				}
				_context.SaveChanges();
				var basketItemss = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(x => x.Status == Enums.ImageStatus.Main)).Where(x=>x.AppUserId==userId).ToList();
				return PartialView("_BasketPartialView", GenerateBasketVM(basketItemss));
			}
			else
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
		private BasketViewModel GenerateBasketVM(List<BasketItem> basketItems)
		{
			BasketViewModel bv = new BasketViewModel();
			foreach (var item in basketItems)
			{
				BasketItemViewModel bi = new BasketItemViewModel
				{
					Count = (int)item.Count,
					Product = item.Product,
				};
				bv.TotalCount = (byte)basketItems.Count;
				bv.Items.Add(bi);
				bv.TotalPrice += (bi.Product.DiscountPercent > 0 ? (((bi.Product.SalePrice * (100 - bi.Product.DiscountPercent)) / 100) * bi.Count) : bi.Product.SalePrice * bi.Count);
			}
			return bv;
		}
		public IActionResult RemoveBasket(int id)
		{
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var basketItem = _context.BasketItems.FirstOrDefault(x=>x.ProductId== id && x.AppUserId==userId);
				if (basketItem!=null)
				{
					_context.BasketItems.Remove(basketItem);
				}
				_context.SaveChanges();
				var basketItems = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(x => x.Status == Enums.ImageStatus.Main)).Where(x => x.AppUserId == userId).ToList();
				return PartialView("_BasketPartialView",GenerateBasketVM(basketItems));
			}
			else
			{
				List<BasketCkItemViewModel> cookieItems = new List<BasketCkItemViewModel>();
				BasketViewModel vm = new BasketViewModel();
				var basketStr = HttpContext.Request.Cookies["Basket"];
				if (basketStr != null)
				{
					cookieItems = JsonConvert.DeserializeObject<List<BasketCkItemViewModel>>(basketStr);
					var item = cookieItems.FirstOrDefault(x => x.ProductId == id);

					if (item != null)
					{
						cookieItems.Remove(item);
					}
					foreach (var ci in cookieItems)
					{
						BasketItemViewModel bi = new BasketItemViewModel
						{
							Count = (int)ci.Count,
							Product = _context.Products.Include(x => x.Images.Where(x => x.Status == Enums.ImageStatus.Main)).FirstOrDefault(x => x.Id == ci.ProductId),

						};

						vm.TotalCount = (byte)cookieItems.Count;
						vm.TotalPrice += (bi.Product.DiscountPercent > 0 ? (bi.Product.SalePrice * (100 - bi.Product.DiscountPercent) / 100) : bi.Product.SalePrice) * bi.Count;
						vm.Items.Add(bi);


					}
					if (cookieItems.Count == 0)
					{
						vm.TotalCount = 0;
					}
					Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));

					return PartialView("_BasketPartialView", vm);

				}
				return NotFound();
			}
		

			

		}



	}
}
