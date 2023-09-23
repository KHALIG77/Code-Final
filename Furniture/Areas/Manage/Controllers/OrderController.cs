using Furniture.DAL;
using Furniture.Enums;
using Furniture.Models;
using Furniture.Services;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xaml.Permissions;

namespace Furniture.Areas.Manage.Controllers
{
	[Area("manage")]
	public class OrderController : Controller
	{
		private readonly FurnutireContext _context;
        private readonly IEmailSender _email;

        public OrderController(FurnutireContext context,IEmailSender email)
		{
			_context = context;
            _email = email;
        }
		public IActionResult Index(int page = 1)
		{
			var query=_context.Orders.AsQueryable();

			return View(PaginatedList<Order>.Create(query,page,2));
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
            _email.Send(order.Email, "Complated", "Sifarisiniz teslim edildi");


            return RedirectToAction("index");
        }
        public IActionResult Shipping(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null) return View("Error");
            order.OrderStatus = OrderStatus.Shipping;
            _context.SaveChanges();
            _email.Send(order.Email, "Shipping", "Sifarisiniz depodan cixdi");


            return RedirectToAction("index");
        }
        public IActionResult Pending(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            if (order == null) return View("Error");
            order.OrderStatus = OrderStatus.Pending;
            _context.SaveChanges();
            _email.Send(order.Email, "Pending", "Sifarisiniz gozlemededir sizinle elaqe saxlanilacaq");


            return RedirectToAction("index");
        }

    }
}
