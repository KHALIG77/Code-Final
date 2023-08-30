using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Furniture.Controllers
{
    public class HomeController : Controller
    {
        private readonly FurnutireContext _context;
        public HomeController(FurnutireContext context)
        {
            _context=context;
        }

        public IActionResult Index()
        {

            HomeViewModel model = new HomeViewModel();
         
            model.Sliders = _context.Sliders.ToList();
            model.Features = _context.Features.Where(x => x.IsShow == true).Take(3).ToList();
            model.Brands = _context.Brands.ToList();
            model.InstagramPhotos = _context.InstagramPhotos.Take(5).ToList();

            if (_context.Products.Any())
            {
                model.FeaturedProducts=_context.Products.Include(x=>x.Category).Include(x=>x.Tags).Include(x=>x.Images).Include(x=>x.Comments).Where(x=>x.IsFeatured==true).Take(8).ToList();
				model.TopProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).Include(x => x.Comments).Where(x =>x.Rate>=4).Take(8).ToList();
                model.BestProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).Include(x => x.Comments).Where(x => x.IsBest == true).Take(8).ToList();


			}

			return View(model);
        }

       
    }
}