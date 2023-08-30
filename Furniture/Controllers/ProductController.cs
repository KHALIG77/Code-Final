
using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Controllers
{
	public class ProductController : Controller
	{
		private readonly FurnutireContext _context;
        public ProductController(FurnutireContext context)
        {
            _context= context;
        }
        public IActionResult index(int id)
		{
		     Product product = _context.Products.Include(x=>x.Comments).Include(x=>x.Images).FirstOrDefault(x=>x.Id==id);


			return PartialView("_ProductDetailPartial",product);
		}
	}
}
