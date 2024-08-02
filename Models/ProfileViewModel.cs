using System.ComponentModel.DataAnnotations;

namespace TL13Shop.Models
{
	public class ProfileViewModel
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		[Display(Name = "Full Name")]
		public string? FullName { get; set; }

		[Display(Name = "Phone Number")]
		[MaxLength(10, ErrorMessage = "Phone number must be 10 digits")]
		[RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number is not in correct format")]
		public string? PhoneNumber { get; set; }

		[Display(Name = "Email")]
		[EmailAddress(ErrorMessage = "Email is not in correct format")]
		public string? Email { get; set; }

		[Display(Name = "Address")]
		public string? Address { get; set; }

		[Display(Name = "Image")]
		public string? ImageUrl { get; set; }

	}
}
