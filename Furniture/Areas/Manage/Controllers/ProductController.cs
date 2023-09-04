using Furniture.DAL;
using Furniture.Helper;
using Furniture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(FurnutireContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.Include(x=>x.Category).Include(x=>x.Material).Include(i=>i.Images).ToList();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors=_context.Colors.ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            if (!_context.Categories.Any(x=>x.Id==product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Please choose correctly");
                return View();
            }
           
            if (product.MainImage == null)
            {
                ModelState.AddModelError("MainImage", "Please add Main Image");
                return View();
            }
            if (product.HoverImage == null)
            {
                ModelState.AddModelError("MainImage", "Please add Main Image");
                return View();
            }
            ProductImage mainImage = new ProductImage
            {
                ImageUrl=FileManager.Save(_env.WebRootPath,"uploads/products",product.MainImage),
                Status=Enums.ImageStatus.Main,
                Product=product,
            };
            product.Images.Add(mainImage);
            ProductImage hoverImage = new ProductImage
            {
                ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/products", product.HoverImage),
                Status = Enums.ImageStatus.Hover,
                Product = product,
            };
            product.Images.Add(hoverImage);
            if (product.AllImages.Count>0)
            {
                foreach (var image in product.AllImages)
                {
                    ProductImage allImages = new ProductImage
                    {
                        ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/products", image),
                        Status = Enums.ImageStatus.Detail,
                        Product = product,
                    };
                    product.Images.Add(allImages);
                }
            }
            if(product.TagIds.Count>0)
            {
                foreach (var tag in product.TagIds)
                {
                    ProductTag productTag = new ProductTag
                    {
                        Product = product,
                        TagId = tag,
                    };
                    product.Tags.Add(productTag);
                }
            }
            if (product.SizeIds.Count > 0)
            {
                foreach (var size in product.SizeIds)
                {
                    ProductSize productSize = new ProductSize
                    {
                        Product = product,
                        SizeId = size,
                    };
                    product.Sizes.Add(productSize);
                }
            }
            if (product.ColorIds.Count > 0)
            {
                foreach (var color in product.ColorIds)
                {
                    ProductColor productColor = new ProductColor
                    {
                        Product = product,
                        ColorId = color,
                    };
                    product.Colors.Add(productColor);
                }
            }
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            return RedirectToAction("index");
        }
    }
}
