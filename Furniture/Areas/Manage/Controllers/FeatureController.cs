using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furniture.Areas.Manage.Controllers
{
	[Area("manage")]
	public class FeatureController : Controller
	{
		private readonly FurnutireContext _context;

		public FeatureController(FurnutireContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			List<Feature> features = _context.Features.ToList();

			return View(features);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Feature feaature)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Fill all cell");
				return View();
			}
			_context.Features.Add(feaature);
			_context.SaveChanges();
			return Redirect("index");
		}
		public IActionResult Delete(int id)
		{
			Feature feature = _context.Features.FirstOrDefault(x=>x.Id == id);	
			if(feature == null)
			{
				return BadRequest();
			}
			else
			{
				_context.Features.Remove(feature);
				_context.SaveChanges();
				return Ok();
			}
		}
	}

}
