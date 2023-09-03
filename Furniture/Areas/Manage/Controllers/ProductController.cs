using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly FurnutireContext _context;

        public ProductController(FurnutireContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.Include(x=>x.Category).Include(i=>i.Images).ToList();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            return RedirectToAction("index");
        }
    }
}
