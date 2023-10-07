using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Furniture.Services
{
	public class LayoutService
	{
		private readonly FurnutireContext _context;
		private readonly IHttpContextAccessor _accessor;
		private readonly UserManager<AppUser> _userManager;

		public LayoutService(FurnutireContext context,IHttpContextAccessor accessor)
        {
			_accessor = accessor;
			_context = context;
		}
		public Dictionary<string, string> GetSettings()
		{
			return _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);	
		}
		public BasketViewModel GetBasket()
		{
			if (_accessor.HttpContext.User.IsInRole("Member") && _accessor.HttpContext.User.Identity.IsAuthenticated)
			{
				var basketItems = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(x => x.Status == Enums.ImageStatus.Main)).Where(x => x.AppUserId == _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
				var VM = new BasketViewModel();
				foreach (var ci in basketItems)
				{
					BasketItemViewModel bi = new BasketItemViewModel
					{
						Count = (int)ci.Count,
						Product = ci.Product,

					};
					VM.Items.Add(bi);
					VM.TotalPrice += (bi.Product.DiscountPercent > 0 ? ((bi.Product.SalePrice * (100 - bi.Product.DiscountPercent)) / 100) : bi.Product.SalePrice * bi.Count);
					VM.TotalCount =(byte)basketItems.Count();
					
				}
				VM.WishlistCount = _context.WishlistItems.Where(x => x.AppUserId == _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).Count();
				return VM;

			}
			
				var basketVM= new BasketViewModel();
				var basketJson = _accessor.HttpContext.Request.Cookies["Basket"];
				if (basketJson!=null)
				{
					var basketItems = JsonConvert.DeserializeObject<List<BasketCkItemViewModel>>(basketJson);
                    foreach (var item in basketItems)
                    {
						BasketItemViewModel basketItem = new BasketItemViewModel
						{
							Product = _context.Products.Include(x=>x.Images).FirstOrDefault(x => x.Id == item.ProductId),
							Count =(int)item.Count,
						};
					   
						basketVM.Items.Add(basketItem);
						basketVM.TotalPrice+= (basketItem.Product.DiscountPercent > 0 ? (basketItem.Product.SalePrice * (100 - basketItem.Product.DiscountPercent) / 100) : basketItem.Product.SalePrice) * basketItem.Count;
					}
				      basketVM.TotalCount =(byte)basketItems.Count;
			}

			return basketVM;
		}

    }
}
