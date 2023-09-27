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
		public IActionResult ModalDetail(int id)
		{
			Product product = _context.Products.Include(x=>x.Comments).Include(x => x.Images).FirstOrDefault(x => x.Id == id);
			if (product==null)
			{
				return View("Error");
			}
			return PartialView("_QuickViewPartialView", product);
		}
		public IActionResult Detail(int id)
		{
			Product product = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Comments).ThenInclude(x=>x.AppUser).Include(x => x.Images).Include(x => x.Material).Include(x => x.Sizes).ThenInclude(s => s.Size).Include(x => x.Colors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);

			if (product == null)
			{
				return View("Error");
			}
			bool accessComment = true;
			if (product.Comments.Any(comment => comment.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)))
			{
				accessComment = false;
			}
			ProductDetailViewModel vm = new ProductDetailViewModel
			{   CommentForm =new Comment { ProductId=product.Id},
				Product = product,
				RelatedProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).ThenInclude(t => t.Tag).Include(x => x.Comments).Include(x => x.Images).Where(x => x.CategoryId == product.CategoryId).ToList(),
				AccessComment = accessComment,
			};

			return View(vm);
		}
		public IActionResult AddToBasket(int id,int count=1,bool detail = false)
		{
			if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var basketItem =_context.BasketItems.FirstOrDefault(x=>x.ProductId==id && x.AppUserId==userId);
				if (basketItem != null)
				{
					if (count>1)
					{
						basketItem.Count = basketItem.Count + count;
					}
					else
					{
                        basketItem.Count++;
                    }
					
				}
				else
				{
					basketItem = new BasketItem
					{
						AppUserId=userId,
						Count=(count>1?count:1),
						ProductId=id,
					};
					_context.BasketItems.Add(basketItem);
				}
				_context.SaveChanges();
				var basketItemss = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.Images.Where(x => x.Status == Enums.ImageStatus.Main)).Where(x=>x.AppUserId==userId).ToList();
				if (detail == true)
				{
					return RedirectToAction("detail", new { id = id });
				}
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
						if (count>1)
						{
							item.Count = item.Count + count;
						}
						else
						{
                            item.Count++;
                        }
						
					}
					else
					{
						item = new BasketCkItemViewModel()
						{
							ProductId = id,
							Count = (count > 1 ? count : 1),
						};
						basketItems.Add(item);
					}
				}
				else
				{
					item = new BasketCkItemViewModel
					{
						ProductId = id,
						Count = (count > 1 ? count : 1),
					};
					basketItems.Add(item);
				}
				HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(basketItems));
				if (detail == true)
				{
					return RedirectToAction("detail", new { id = id });
				}
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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CommentForm(Comment comment)
		{
			if (!User.Identity.IsAuthenticated && !User.IsInRole("Member"))
			{
				return RedirectToAction("login", "account", new { returUrl = Url.Action("detail", "product", new { id = comment.ProductId }) });
			}
			if (!ModelState.IsValid)
			{
				Product product = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Comments).Include(x => x.Images).Include(x => x.Material).Include(x => x.Sizes).ThenInclude(s => s.Size).Include(x => x.Colors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == comment.ProductId);

				if (product == null)
				{
					return View("Error");
				}
				ProductDetailViewModel vm = new ProductDetailViewModel
				{
					Product =product,
					CommentForm = new Comment { ProductId = product.Id },
					RelatedProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).ThenInclude(t => t.Tag).Include(x => x.Comments).Include(x => x.Images).Where(x => x.CategoryId == product.CategoryId).ToList(),


				};
				vm.CommentForm = comment;
				return View("Detail", vm);

			}
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Comment comme = new Comment
			{
				AppUserId = userId,
				CreatedAt = DateTime.UtcNow.AddHours(4),
				ProductId = comment.ProductId,
				Rate = (byte)comment.Rate,
				CommentText = comment.CommentText,


			};
			


			_context.Comments.Add(comme);
			
			Product product1 = _context.Products.Include(x => x.Comments).FirstOrDefault(x => x.Id == comment.ProductId);
			product1.Rate = (byte)Math.Ceiling((decimal)(product1.Comments.Average(x => x.Rate))) ;

			_context.SaveChanges();

			return RedirectToAction("detail", new { id = comment.ProductId });

		}


	}
}
