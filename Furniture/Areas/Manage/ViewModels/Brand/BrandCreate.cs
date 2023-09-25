using Furniture.Attiributes.ValidationAttiribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furniture.Areas.Manage.ViewModels.Brand
{
    public class BrandCreate
    {
        [Required]
        [AllowFileType("image/png", "image/jpeg", "image/jpg")]
        [FileSize(1000000)]
        public IFormFile ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
