using Furniture.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Reflection.Emit;

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
        public DbSet<Size> Sizes {get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductSize> ProductSizes {get; set; }
		public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasMany(p => p.Products).WithOne(c => c.Category).HasForeignKey(f => f.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Tag>().HasMany(p=>p.Products).WithOne( t=> t.Tag).HasForeignKey(f => f.TagId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Material>().HasMany(p => p.Products).WithOne(c => c.Material).HasForeignKey(f => f.MaterialId).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
