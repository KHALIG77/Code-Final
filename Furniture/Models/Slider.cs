using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
    public class Slider
    {
        
        public int Id { get; set; }

        public byte Order {get; set; }
        [Required]
        [MaxLength(100)]

        public string Image {get; set;}
        [Required]
        [MaxLength(50)]
        public string Title {get; set;}
        [Required]
        [MaxLength(50)]
        public string Description { get; set;}
        [Required]
        public string BtnUrl {get; set;}
        public bool MainSlider { get; set; }
        
    }
}
