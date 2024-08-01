using System.ComponentModel.DataAnnotations;

namespace TL13Shop.Models
{
	public class LoginVM
	{ 
		[Display(Name = "Username")]
		[Required(ErrorMessage = "Username is required")]
		[MaxLength(20, ErrorMessage = "Username must be less than 20 characters")]
		[MinLength(5, ErrorMessage = "Username must be more than 5 characters")]
		public string UserName { get; set; }
		[Display(Name = "Password")]
		[Required(ErrorMessage = "Password is required")]
		[MaxLength(20, ErrorMessage = "Password must be less than 20 characters")]
		[MinLength(5, ErrorMessage = "Password must be more than 5 characters")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
