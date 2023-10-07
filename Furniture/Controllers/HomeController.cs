using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;

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
         
            model.Sliders = _context.Sliders.OrderByDescending(x=>x.Order).Where(x=>x.ForAbout==false).ToList();
            model.Features = _context.Features.OrderByDescending(x=>x.Id).Take(3).ToList();
            model.Brands = _context.Brands.Take(5).ToList();
            model.InstagramPhotos = _context.InstagramPhotos.OrderByDescending(x=>x.Id).Take(7).ToList();
            model.FeaturedProducts=_context.Products.Include(x=>x.Category).Include(x=>x.Tags).Include(x=>x.Images).Include(x=>x.Comments).Where(x=>x.IsFeatured==true).Take(8).ToList();
		    model.TopProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).Include(x => x.Comments).Where(x =>x.Rate>=4).Take(8).ToList();
            model.BestProducts = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).Include(x => x.Comments).Where(x => x.IsBest == true).Take(8).ToList();


			

			return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe(string email)
        {
			string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
			
			if (email == null)
            {
				return Json(new { status = 0 });
				
			}
            else if(!Regex.IsMatch(email, emailPattern))
            {
				return Json(new { status = 1 });

			}
            else if(_context.Subscribes.Any(x=>x.Email==email)) 
            {
				return Json(new { status = 2 });
			}
            else
            {
                Subscribe sub = new Subscribe
                {
                    Email = email,
                };
                _context.Subscribes.Add(sub);
                _context.SaveChanges();
                return Json(new { status = 3 });

			}
			

		
           
           
            
        }
       
    }
}