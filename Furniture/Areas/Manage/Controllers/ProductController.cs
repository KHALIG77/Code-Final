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
    }
}
