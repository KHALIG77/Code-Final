using Furniture.Areas.Manage.ViewModels.Category;
using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Areas.Manage.Controllers
{
	[Area("manage")]
	public class CategoryController:Controller
	{
		private readonly FurnutireContext _context;

		public CategoryController(FurnutireContext context)
        {
		_context = context;
		}
        public IActionResult Index()
		{
			List<Category> categories = _context.Categories.Include(x=>x.Products).ToList();

			return View(categories);

		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CategoryCreate category)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("Name", "Please write correctly");
				return View();
			}
			if (_context.Categories.Any(x=>x.Name==category.Name))
			{
				ModelState.AddModelError("Name", "This name already taken");
				return View();
			}
			Category cate= new Category
			{
                 Name = category.Name
			};

			_context.Categories.Add(cate);
			_context.SaveChanges();
			return RedirectToAction("index");
		}
	
		public IActionResult Delete(int id)
		{
			Category category=_context.Categories.FirstOrDefault(x=>x.Id==id);
			if(category==null)
			{
				return BadRequest();
			}
			_context.Categories.Remove(category);
			_context.SaveChanges();
			return Ok();
		}
	}
}
