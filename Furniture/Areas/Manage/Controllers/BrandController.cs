using Furniture.Areas.Manage.ViewModels.Brand;
using Furniture.DAL;
using Furniture.Helper;
using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
   
    public class BrandController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(FurnutireContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Brand> brands = _context.Brands.ToList();

            
            return View(brands);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BrandCreate brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (brand.ImageUrl == null)
            {
                ModelState.AddModelError("ImageFile", "Please add image");
                return View();
            }

            Brand newBrand = new Brand
            {
                Image = FileManager.Save(_env.WebRootPath, "uploads/brands", brand.ImageUrl),
                Name = brand.Name

            };


          
            _context.Brands.Add(newBrand);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return BadRequest();

            }
            else
            {
                string imageName = brand.Image;
                _context.Brands.Remove(brand);
                _context.SaveChanges();

                FileManager.Delete(_env.WebRootPath, "uploads/brands", imageName);
                return Ok();
            }


        }
    }
}
