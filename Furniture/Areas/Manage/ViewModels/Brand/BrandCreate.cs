using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furniture.Areas.Manage.ViewModels.Brand
{
    public class BrandCreate
    {
        [Required]
        [NotMapped]
        public IFormFile ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
