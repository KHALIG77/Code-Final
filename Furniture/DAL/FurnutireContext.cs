using Furniture.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Furniture.DAL
{
    public class FurnutireContext:IdentityDbContext
    {
        public FurnutireContext(DbContextOptions<FurnutireContext> options):base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }   
        public DbSet<Feature> Features { get; set; }
        public DbSet<Brand> Brands {get; set; }
        public DbSet<InstagramPhoto> InstagramPhotos { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Product>Products { get; set; } 
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
