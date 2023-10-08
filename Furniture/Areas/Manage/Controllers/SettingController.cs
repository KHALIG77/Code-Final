using Furniture.Areas.Manage.ViewModels;
using Furniture.Areas.Manage.ViewModels.Setting;
using Furniture.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Furniture.Areas.Manage.Controllers
{
	[Area("manage")]
	[Authorize(Roles = "Admin")]
	public class SettingController : Controller
	{
		private readonly FurnutireContext _context;

		public SettingController(FurnutireContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			SettingViewModel vm = new SettingViewModel();

			vm.Address =_context.Settings.FirstOrDefault(x => x.Key == "Address").Value;
			vm.Number1 = _context.Settings.FirstOrDefault(x => x.Key == "Number1").Value;
			vm.Number2 = _context.Settings.FirstOrDefault(x => x.Key == "Number2").Value;
			vm.Email = _context.Settings.FirstOrDefault(x => x.Key == "Email").Value;

		
			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(SettingViewModel model)
		{
			_context.Settings.FirstOrDefault(x => x.Key == "Address").Value = model.Address;
			_context.Settings.FirstOrDefault(x => x.Key == "Number1").Value = model.Number1;
			_context.Settings.FirstOrDefault(x => x.Key == "Number2").Value = model.Number2;
			_context.Settings.FirstOrDefault(x => x.Key == "Email").Value = model.Email;
			_context.SaveChanges();

			return RedirectToAction("index");

		}
	}
}
