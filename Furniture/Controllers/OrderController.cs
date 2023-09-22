using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Furniture.Controllers
{
	public class OrderController : Controller
	{
		private readonly FurnutireContext _context;
		private readonly UserManager<AppUser> _userManager;

		public OrderController(FurnutireContext context,UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(OrderFormViewModel orderVM)
		{
			if (!User.Identity.IsAuthenticated || !User.IsInRole("Member"))
			{
				if (string.IsNullOrEmpty(orderVM.FullName))
				{
					ModelState.AddModelError("FullName", "FullName is required");
					OrderViewModel vm = new OrderViewModel();
					vm.Items = GetCheckoutItems();
					vm.Form = orderVM;
					vm.TotalPrice = vm.TotalPrice = vm.Items.Any() ? vm.Items.Sum(x => x.Price * x.Count) : 0;
					return View("Checkout", vm);
				}
				if (string.IsNullOrEmpty(orderVM.Email))
				{
					ModelState.AddModelError("Email", "Email is required");
					OrderViewModel vm = new OrderViewModel();
					vm.Items = GetCheckoutItems();
					vm.Form = orderVM;
					vm.TotalPrice = vm.Items.Any() ? vm.Items.Sum(x => x.Price * x.Count) : 0;

					return View("Checkout", vm);
				}
			}
			//if (!ModelState.IsValid)
			//{
			//	OrderViewModel vm = new OrderViewModel();
			//	vm.Items = GetCheckoutItems();
			//	vm.Form = orderVM;
			//	vm.TotalPrice = vm.Items.Any() ? vm.Items.Sum(x => x.Price * x.Count) : 0;

			//	return View("Checkout", vm);
			//}


			Order order = new Order()
			{
				Address = orderVM.Address,
				Note = orderVM.Note,
				OrderStatus = Enums.OrderStatus.Pending,
				CreateAt = DateTime.UtcNow.AddHours(4),
				City=orderVM.City,
				Apartment=orderVM.Apartment,
				Phone=orderVM.Phone,
			};
			var items = GetCheckoutItems();
			foreach (var item in items)
			{
				Product product = _context.Products.Find(item.ProductId);
				OrderItem orderItem = new OrderItem
				{
					ProductId = item.ProductId,
					DiscountPercent = product.DiscountPercent,
					UnitCostPrice = product.CostPrice,
					UnitPrice = product.DiscountPercent > 0 ? ((product.SalePrice * (100 - product.DiscountPercent)) / 100) : product.SalePrice,
					Count = item.Count,

				};
				order.OrderItems.Add(orderItem);
			}
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
				order.FullName = user.FullName ?? orderVM.FullName;
				order.Email = user.Email;
				order.AppUserId = user.Id;

				ClearDbBasket(user.Id);

			}

			else
			{
				order.FullName = orderVM.FullName;
				order.Email = orderVM.Email;
				Response.Cookies.Delete("Basket");
			}
			_context.Orders.Add(order);
			_context.SaveChanges();
			return RedirectToAction("index", "home");


		}
		public async Task<IActionResult> Checkout()
		{
			OrderViewModel orderVM = new OrderViewModel();
			orderVM.Items = GetCheckoutItems();
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
				orderVM.Form = new OrderFormViewModel
				{
					Address = user.Address,
					Email = user.Email,
					FullName = user.FullName,

				};
				orderVM.TotalPrice = orderVM.Items.Any() ? orderVM.Items.Sum(x => x.Price * x.Count) : 0;
				return View(orderVM);


			}
			orderVM.TotalPrice = orderVM.Items.Any() ? orderVM.Items.Sum(x => x.Price * x.Count) : 0;

			return View(orderVM);
		}
		private List<CheckoutItem> GetCheckoutItems()
		{
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				return CheckoutItemsFromDb(userId);
			}
			else
			{
				return CheckoutItemsFromCookie();
			}
		}
		private List<CheckoutItem> CheckoutItemsFromDb(string userId)
		{
			return _context.BasketItems.Include(x => x.Product).ThenInclude(x=>x.Images).Where(x => x.AppUserId == userId).Select(x => new CheckoutItem
			{
				Image=x.Product.Images.FirstOrDefault(x=>x.Status ==Enums.ImageStatus.Main).ImageUrl,
				ProductId = x.ProductId,
				Count = x.Count,
				Name = x.Product.Name,
				Price = x.Product.DiscountPercent > 0 ? ((x.Product.SalePrice * (100 - x.Product.DiscountPercent) / 100)) : x.Product.SalePrice

			}).ToList();
		}
		private List<CheckoutItem> CheckoutItemsFromCookie()
		{
			List<CheckoutItem> checkoutItems = new List<CheckoutItem>();
			var basketStr = Request.Cookies["Basket"];
			if (basketStr != null)
			{
				List<BasketCkItemViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketCkItemViewModel>>(basketStr);
				foreach (var item in cookieItems)
				{
					Product product = _context.Products.Include(x=>x.Images).FirstOrDefault(x => x.Id == item.ProductId);

					CheckoutItem checkoutItem = new CheckoutItem()
					{
						Image=product.Images.FirstOrDefault(x=>x.Status==Enums.ImageStatus.Main).ImageUrl,
						Count = (int)item.Count,
						ProductId = product.Id,
						Name = product.Name,
						Price = (decimal)(product.DiscountPercent > 0 ? ((product.SalePrice * (100 - product.DiscountPercent) / 100)) : product.SalePrice)

					};
					checkoutItems.Add(checkoutItem);

				}

			}
			return checkoutItems;
		}
		private void ClearDbBasket(string userId)
		{
			_context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == userId).ToList());
			_context.SaveChanges();
		}
	}
}
