using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Furniture.ViewModels
{
	public class OrderFormViewModel
	{

		[MaxLength(100)]
		[Required]
		public string FullName {get; set;}
		[MaxLength(100)]
		[Required]
		public string City {get; set;}
		[MaxLength(100)]
		[Required]
		public string Address {get; set;}
		[MaxLength(100)]
		
		public string Apartment {get; set;}
		[MaxLength(100)]
		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email {get; set;}
		[Required]
		[MaxLength(30)]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set;}
		[MaxLength(255)]
		public string Note {get; set;}



	}
}
