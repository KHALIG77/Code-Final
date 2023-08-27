using Furniture.DAL;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            HomeViewModel model = new HomeViewModel()
            {
                Sliders=_context.Sliders.ToList(),
                Features=_context.Features.Where(x=>x.IsShow==true).Take(3).ToList(),
                Brands=_context.Brands.ToList(),
                InstagramPhotos=_context.InstagramPhotos.ToList(),
            };

            return View(model);
        }

       
    }
}