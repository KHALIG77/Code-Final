using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
    public class Material
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(3)]
        public string Name { get; set; }    
        public List<Product> Products { get; set; } 
    }
}
