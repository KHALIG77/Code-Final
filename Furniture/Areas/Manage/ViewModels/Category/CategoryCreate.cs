

using System.ComponentModel.DataAnnotations;
namespace Furniture.Areas.Manage.ViewModels.Category
{
    public class CategoryCreate
    {
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Name {get; set;}

    }
}
