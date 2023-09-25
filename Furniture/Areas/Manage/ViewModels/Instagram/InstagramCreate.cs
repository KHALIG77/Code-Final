using Furniture.Attiributes.ValidationAttiribute;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Areas.Manage.ViewModels.Instagram
{
    public class InstagramCreate
    {
        [Required]
        public string Url {get; set;}
        [Required]
        [AllowFileType("image/png", "image/jpeg", "image/jpg")]
        [FileSize(1000000)]
        public IFormFile ImageUrl { get; set;}
    }
}
