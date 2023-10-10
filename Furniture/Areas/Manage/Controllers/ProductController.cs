using Furniture.DAL;
using Furniture.Helper;
using Furniture.Models;
using Furniture.Services;
using Furniture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Printing;

namespace Furniture.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin,Stock")]
    public class ProductController : Controller
    {
        private readonly FurnutireContext _context;
        private readonly IWebHostEnvironment _env;
		private readonly IEmailSender _email;

		public ProductController(FurnutireContext context,IWebHostEnvironment env,IEmailSender emailService)
        {
            _context = context;
            _env = env;
			_email = emailService;
		}
        public IActionResult Index(int page = 1,string search=null)
        {
            var query = _context.Products.Include(x => x.Category).Include(x => x.Material).Include(i => i.Images).AsQueryable();
            if (search!=null)
            {
                query=query.Where(x=>x.Name.Contains(search));
            }
            ViewBag.Search = search;
            ViewBag.Page = page;
            return View(PaginatedList<Product>.Create(query,page,6));
        }
        public async Task<IActionResult> Create()
        {
			ViewBag.Brands = _context.Brands.ToList();
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
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
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
            
            product.Rate = 5;
            _context.Products.Add(product);
            _context.SaveChanges();
            foreach (var item in _context.Subscribes)
            {
                _email.Send(item.Email,"New Product", "Our new product has arrived. Visit the site to view the new product",false);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
			ViewBag.Brands = _context.Brands.ToList();
			ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            Product product = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).Include(x => x.Material).Include(x=>x.Colors).Include(x=>x.Sizes).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return View("Error");
            }
            product.TagIds=product.Tags.Select(x=>x.TagId).ToList();
            product.SizeIds = product.Sizes.Select(x => x.SizeId).ToList();
            product.ColorIds = product.Colors.Select(x => x.ColorId).ToList();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
			ViewBag.Brands = _context.Brands.ToList();
			ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors = _context.Colors.ToList();

            Product existProduct = _context.Products.Include(x => x.Category).Include(x => x.Tags).Include(x => x.Images).Include(x => x.Material).Include(x => x.Colors).Include(x => x.Sizes).FirstOrDefault(x => x.Id == product.Id);
            if (existProduct == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View(existProduct);
            }
            if (product.CategoryId!=existProduct.CategoryId && !_context.Categories.Any(x=>x.Id==product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found");
                return View();
            }
            if (product.MaterialId != existProduct.MaterialId && !_context.Materials.Any(x => x.Id == product.MaterialId))
            {
                ModelState.AddModelError("MaterialId", "Material not found");
                return View();
            }
            string oldMainPhoto = null;
            if(product.MainImage!=null)
            {
                ProductImage mainImage = existProduct.Images.FirstOrDefault(x => x.Status == Enums.ImageStatus.Main);
                oldMainPhoto = mainImage.ImageUrl;
                if(mainImage != null)
                {
                    mainImage.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/products", product.MainImage);
                }
            }
            string oldHoverPhoto = null;
            if (product.HoverImage != null)
            {
                ProductImage hoverImage = existProduct.Images.FirstOrDefault(x => x.Status == Enums.ImageStatus.Hover);
                oldHoverPhoto = hoverImage.ImageUrl;
                if (hoverImage != null)
                {
                    hoverImage.ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/products", product.HoverImage);
                }
            }
            existProduct.Tags.RemoveAll(x => !product.TagIds.Contains(x.Id));
            var newTagIds = product.TagIds.Where(x => !existProduct.Tags.Any(y => y.TagId == x));

            foreach (var tagId in newTagIds)
            {
                ProductTag tags = new ProductTag()
                {
                    TagId = tagId,

                };
                existProduct.Tags.Add(tags);
            }
            existProduct.Sizes.RemoveAll(x => !product.SizeIds.Contains(x.Id));
           var newSizeIds = product.SizeIds.Where(x => !existProduct.Sizes.Any(y => y.SizeId == x));

            foreach (var sizeId in newSizeIds)
            {
                ProductSize sizes = new ProductSize()
                {
                    SizeId = sizeId,

                };
                existProduct.Sizes.Add(sizes);
            }
            existProduct.Colors.RemoveAll(x => !product.ColorIds.Contains(x.Id));
            var newColorIds = product.ColorIds.Where(x => !existProduct.Colors.Any(y => y.ColorId == x));

            foreach (var colorId in newColorIds)
            {
                ProductColor colors = new ProductColor()
                {
                    ColorId = colorId,

                };
                existProduct.Colors.Add(colors);
            }
            var removedDetailImages = existProduct.Images.FindAll(x=>x.Status==Enums.ImageStatus.Detail && !product.ImageIds.Contains(x.Id));
            existProduct.Images.RemoveAll(x => x.Status == Enums.ImageStatus.Detail && !product.ImageIds.Contains(x.Id));
            foreach (var img in product.AllImages)
            {
                ProductImage image = new ProductImage
                {   ProductId=existProduct.Id,
                    Status = Enums.ImageStatus.Detail,
                    ImageUrl = FileManager.Save(_env.WebRootPath, "uploads/products", img)
                };
                existProduct.Images.Add(image);
            }
            existProduct.BrandId = product.BrandId;
            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.CostPrice= product.CostPrice;
            existProduct.SalePrice= product.SalePrice;
            existProduct.DiscountPercent= product.DiscountPercent;
            existProduct.CategoryId= product.CategoryId;
            existProduct.MaterialId= product.MaterialId;
            existProduct.IsBest= product.IsBest;
            existProduct.IsFeatured= product.IsFeatured;
            existProduct.IsStock= product.IsStock;
            existProduct.Detail = product.Detail;

            _context.SaveChanges();
            if(oldMainPhoto!=null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/products", oldMainPhoto);
            }
            if (oldHoverPhoto != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/products", oldHoverPhoto);
            }
            if (removedDetailImages.Any())
            {
                FileManager.DeleteAll(_env.WebRootPath, "uploads/products",removedDetailImages.Select(x=>x.ImageUrl).ToList());
            }

            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            Product product = _context.Products.Include(x=>x.Images).FirstOrDefault(x=>x.Id==id);
            if(product == null)
            {
                return BadRequest();
            }
            else
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                if (product.Images.Any(x => x.Status == Enums.ImageStatus.Main))
                {
                    FileManager.Delete(_env.WebRootPath,"uploads/products",product.Images.FirstOrDefault(x=>x.Status==Enums.ImageStatus.Main).ImageUrl);
                }
                if (product.Images.Any(x => x.Status == Enums.ImageStatus.Hover))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/products", product.Images.FirstOrDefault(x => x.Status == Enums.ImageStatus.Hover).ImageUrl);
                }
                if (product.Images.Any(x => x.Status == Enums.ImageStatus.Detail))
                {
                    FileManager.DeleteAll(_env.WebRootPath, "uploads/products", product.Images.Where(x => x.Status == Enums.ImageStatus.Detail).Select(x=>x.ImageUrl).ToList());
                }
              

                return Ok();

            }
           
        }
    }
}
