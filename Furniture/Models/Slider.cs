using Furniture.Attiributes.ValidationAttiribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Furniture.Models
{
    public class Slider
    {
        
        public int Id { get; set; }

        public byte Order {get; set; }
      
        [MaxLength(100)]

        public string? Image {get; set;}
        [Required]
        [MaxLength(50)]
        public string Title {get; set;}
        [Required]
        [MaxLength(50)]
        public string Description { get; set;}
        [Required]
        public string BtnUrl {get; set;}
        public bool MainSlider { get; set; }
        [NotMapped]
        [AllowFileType("image/png", "image/jpeg", "image/jpg")]
        [FileSize(1000000)]
        public IFormFile ImageSlide { get; set; }
        
    }
}
