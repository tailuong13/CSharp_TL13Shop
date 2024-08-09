using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TL13Shop.Models
{
    public class CheckoutViewModel
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string? Note { get; set; }
        [BindNever]
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
	}
}
