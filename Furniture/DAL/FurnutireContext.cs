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
    }
}
