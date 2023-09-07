using Furniture.Areas.Manage.ViewModels.Category;
using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    public class MaterialController : Controller
    {
        private readonly FurnutireContext _context;

        public MaterialController(FurnutireContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Material> materials = _context.Materials.Include(x => x.Products).ToList();

            return View(materials);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Please write correctly");
                return View();
            }
            if (_context.Materials.Any(x => x.Name == material.Name))
            {
                ModelState.AddModelError("Name", "This name already taken");
                return View();
            }
            Material mate = new Material
            {
                Name = material.Name
            };

            _context.Materials.Add(mate);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Material material = _context.Materials.FirstOrDefault(x => x.Id == id);
            if (material == null)
            {
                return BadRequest();
            }
            _context.Materials.Remove(material);
            _context.SaveChanges();
            return Ok();
        }

        public IActionResult Edit(int id)
        {
            Material material = _context.Materials.FirstOrDefault(x => x.Id == id);
            if (material == null)
            {
                return View("Error");
            }
            return View(material);
        }
        [HttpPost]
        public IActionResult Edit(Material material)
        {
            Material existMaterial = _context.Materials.FirstOrDefault(x => x.Id == material.Id);
            if (existMaterial == null)
            {
                return View("Error");
            }
            if (existMaterial.Name != material.Name && _context.Materials.Any(x => x.Name == material.Name))
            {
                ModelState.AddModelError("Name", "This name already taken");
                return View();
            }
            existMaterial.Name = material.Name;
            _context.SaveChanges();
            return RedirectToAction("index");

        }

    }
}

