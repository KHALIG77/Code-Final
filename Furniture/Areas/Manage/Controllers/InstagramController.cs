using Furniture.Areas.Manage.ViewModels.Brand;
using Furniture.Areas.Manage.ViewModels.Instagram;
using Furniture.DAL;
using Furniture.Helper;
using Furniture.Models;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
	[Authorize(Roles = "Admin")]
	public class InstagramController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly IWebHostEnvironment _env;

        public InstagramController(FurnutireContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.InstagramPhotos.AsQueryable();

            return View(PaginatedList<InstagramPhoto>.Create(query,page,2));
            
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InstagramCreate instagram)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (instagram.ImageUrl == null)
            {
                ModelState.AddModelError("ImageFile", "Please add image");
                return View();
            }

            InstagramPhoto photo = new InstagramPhoto
            {
                Image = FileManager.Save(_env.WebRootPath, "uploads/instagram", instagram.ImageUrl),
                Url =instagram.Url

            };



            _context.InstagramPhotos.Add(photo);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            InstagramPhoto photo = _context.InstagramPhotos.FirstOrDefault(b => b.Id == id);
            if (photo == null)
            {
                return BadRequest();

            }
            else
            {
                string imageName = photo.Image;
                _context.InstagramPhotos.Remove(photo);
                _context.SaveChanges();

                FileManager.Delete(_env.WebRootPath, "uploads/instagram", imageName);
                return Ok();
            }


        }
    }
}
