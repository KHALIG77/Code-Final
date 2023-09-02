using System.ComponentModel.DataAnnotations;

namespace Furniture.Areas.Manage.ViewModels.Tag
{
    public class CreateTag
    {
        [Required]
        [MaxLength(60)]
        [MinLength(3)]
        public string Name { get; set; }

    }
}
