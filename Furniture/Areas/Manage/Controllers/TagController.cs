using Furniture.Areas.Manage.ViewModels.Tag;
using Furniture.DAL;
using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class TagController : Controller
    {
        private readonly FurnutireContext _context;

        public TagController(FurnutireContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Tag> tags=_context.Tags.Include(x=>x.Products).ToList();

            return View(tags);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Tag data = await _context.Tags.FirstOrDefaultAsync(x=>x.Id == id);
            if(data== null)
            {
                return View("Error");
            }
            _context.Tags.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");

        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTag tag)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "Please write correctly");
                return View();
            }
            if (_context.Tags.Any(x=>x.Name==tag.Name))
            {
                ModelState.AddModelError("Name", "This name already taken");
                return View();

            }
            Tag data = new Tag { Name = tag.Name };

            await _context.Tags.AddAsync(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");

        }
        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag == null)
            {
                return View("Error");
            }
            return View(tag);
        }
        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            Tag existTag = _context.Tags.FirstOrDefault(x => x.Id == tag.Id);
            if (existTag == null)
            {
                return View("Error");
            }
            if (existTag.Name != tag.Name && _context.Categories.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "This name already taken");
                return View();
            }
            existTag.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
