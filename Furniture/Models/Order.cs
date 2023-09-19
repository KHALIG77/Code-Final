using Furniture.Enums;
using System.ComponentModel.DataAnnotations;

namespace Furniture.Models
{
	public class Order
	{
		public int Id { get; set; }
		[MaxLength(100)]
		[Required]
		public string FullName { get; set; }
		[MaxLength(100)]
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email {get; set; }
		[MaxLength(100)]
		[Required]
		public string Address { get; set; }
		[MaxLength(100)]
		[Required]
		public string City { get; set; }
		[MaxLength(100)]
		public string Apartment { get; set; }
		[Required]
		[MaxLength(30)]
		[DataType(DataType.PhoneNumber)]
		public string Phone {get; set; }
		[MaxLength(255)]
		public string Note {get; set; }
		[Required]
		public OrderStatus OrderStatus { get; set; }
		public string AppUserId {get; set; }
		public AppUser AppUser { get; set; }
		[Required]
		public DateTime CreateAt { get; set; }
		public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

	}
}
