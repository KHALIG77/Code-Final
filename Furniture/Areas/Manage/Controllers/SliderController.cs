using Furniture.DAL;
using Furniture.Helper;
using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
	[Authorize(Roles = "Admin")]
	public class SliderController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(FurnutireContext context ,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();

            return View(sliders);
        }
        public IActionResult Create()
        {
            if (_context.Sliders.Any())
            {
                Slider slider = _context.Sliders.OrderByDescending(x => x.Order).FirstOrDefault();
                ViewBag.Order = slider.Order + 1;

            }
            else
            {
                ViewBag.Order = 0;
            }
           

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider sliderDTO)
        {
            if (_context.Sliders.Any())
            {
                Slider sliderforOrder = _context.Sliders.OrderByDescending(x => x.Order).FirstOrDefault();
                ViewBag.Order = sliderforOrder.Order + 1;

            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            if(sliderDTO.ImageSlide==null)
            {
                ModelState.AddModelError("ImageSlide", "Please Add Photo");
                return View();
            }
            if(sliderDTO.Order!=ViewBag.Order)
            {
                if (_context.Sliders.Any(x=>x.Order==sliderDTO.Order))
                {
                    ModelState.AddModelError("Order", "Order Number already used");
                    return View();
                }
            }
            Slider slider = new Slider()
            {
                MainSlider = sliderDTO.MainSlider,
                BtnUrl = sliderDTO.BtnUrl,
                Title = sliderDTO.Title,
                Description = sliderDTO.Description,
                Image = FileManager.Save(_env.WebRootPath,"uploads/sliders",sliderDTO.ImageSlide),
                Order=sliderDTO.Order,
                ForAbout = sliderDTO.ForAbout,
            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            
            return RedirectToAction("index");
        }
        
        public IActionResult Edit(int id)
        {
            Slider slider =_context.Sliders.FirstOrDefault(x=>x.Id==id);
            if(slider == null)
            {
                return View("Error");
            }

            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider sliderEditDTO)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(x=>x.Id==sliderEditDTO.Id);
            if(existSlider == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View(existSlider);
            }
            if (_context.Sliders.Any(x=>x.Order==sliderEditDTO.Order) && existSlider.Order!=sliderEditDTO.Order )
            {
                ModelState.AddModelError("Order", "This Order already taken");
                return View(existSlider);
            }
            string oldImage = null;
            if (sliderEditDTO.ImageSlide != null)
            {
                oldImage = existSlider.Image;
                existSlider.Image = FileManager.Save(_env.WebRootPath, "uploads/sliders", sliderEditDTO.ImageSlide);
            }
            
            existSlider.Title=sliderEditDTO.Title;
            existSlider.Description=sliderEditDTO.Description;
            existSlider.Order=sliderEditDTO.Order;
            existSlider.BtnUrl=sliderEditDTO.BtnUrl;
            existSlider.MainSlider=sliderEditDTO.MainSlider;
            existSlider.ForAbout=sliderEditDTO.ForAbout;

            _context.SaveChanges();
            
            if(oldImage!= null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/sliders",oldImage);
            }
            return RedirectToAction("index");
            
        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
            {
                return BadRequest();
            }
            string sliderImage = slider.Image;

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            if(sliderImage != null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/sliders",sliderImage);
            }
            return Ok();
        

        }
    }
}
