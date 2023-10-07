
using Furniture.DAL;
using Furniture.Services;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Furniture.Controllers
{
	public class ContactController : Controller
	{
		private readonly FurnutireContext _context;
		private readonly IEmailSender _emailSender;

		public 	ContactController(FurnutireContext context,IEmailSender emailSender)
		{
		    _context = context;
			_emailSender = emailSender;
			
		}
		public IActionResult Index()
		{
			ViewBag.Phone = _context.Settings.FirstOrDefault(x => x.Key == "Number1").Value;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(CustomerQuestion question)
		{
			string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

			ViewBag.Phone = _context.Settings.FirstOrDefault(x => x.Key == "Number1").Value;
			if (question.Email!=null)
			{
				if (!Regex.IsMatch(question.Email, emailPattern))
				{
					ModelState.AddModelError("Email", "Please write correctly");
					return View();

				}
			}
			
			if (!ModelState.IsValid) 
			{
				return View();
			}
			_emailSender.Send("khaligfm@code.edu.az", question.Email, question.Text, true);

		

			return View();

		}
	}
}
