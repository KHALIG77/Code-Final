using Furniture.DAL;
using Furniture.Enums;
using Furniture.Models;
using Furniture.Services;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xaml.Permissions;

namespace Furniture.Areas.Manage.Controllers
{
	[Area("manage")]
    [Authorize(Roles = "SuperAdmin,Stock")]

    public class OrderController : Controller
	{
		private readonly FurnutireContext _context;
        private readonly IEmailSender _email;

        public OrderController(FurnutireContext context,IEmailSender email)
		{
			_context = context;
            _email = email;
        }
		public IActionResult Index(int page = 1,int orderstatus=0,string search=null)
		{
            var query = _context.Orders.Include(x=>x.AppUser).AsQueryable();
            if (orderstatus != 0)
            {
                query = query.Where(x => (int)x.OrderStatus == orderstatus);

            }
            if (search != null)
            {
                query = query.Where(x =>x.FullName.Contains(search));
            }
            ViewBag.Search = search;
            ViewBag.OrderStatus = orderstatus;


            return View(PaginatedList<Order>.Create(query,page,5));
		}
		public IActionResult Detail(int id)
		{
			Order order  =_context.Orders.Include(x=>x.AppUser).Include(x=>x.OrderItems).ThenInclude(x=>x.Product).FirstOrDefault(x=>x.Id==id);
			if (order == null)
			{
				return View("Error");
			}
			else
			{
                return View(order);

            }

        }
        public IActionResult Completed(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null) return View("Error");
            order.OrderStatus = OrderStatus.Completed;
            _context.SaveChanges();
            _email.Send(order.Email, "Complated", "Your order has been delivered",false);


            return RedirectToAction("index");
        }
        public IActionResult Shipping(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null) return View("Error");
            order.OrderStatus = OrderStatus.Shipping;
            _context.SaveChanges();
            _email.Send(order.Email, "Shipping", "Your order has left the warehouse and is being sent to you", false);


            return RedirectToAction("index");
        }
        public IActionResult Pending(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null) return View("Error");
            order.OrderStatus = OrderStatus.Pending;
            _context.SaveChanges();
            _email.Send(order.Email, "Pending", "Your order has been received. You will be contacted", false);


            return RedirectToAction("index");
        }

    }
}
