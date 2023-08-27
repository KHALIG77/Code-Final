using Furniture.Models;

namespace Furniture.ViewModels
{
    public class HomeViewModel
    {
       public List<Slider> Sliders { get; set; }
       public List<Feature> Features { get; set; }
       public List<Brand> Brands { get; set; }
       public List<InstagramPhoto> InstagramPhotos { get; set; }
    }
}
