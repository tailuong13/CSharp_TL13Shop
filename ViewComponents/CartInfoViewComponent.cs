using Microsoft.AspNetCore.Mvc;
using TL13Shop.Helpers;
using TL13Shop.Models;

namespace TL13Shop.ViewComponents
{
    public class CartInfoViewComponent : ViewComponent
    { 
        public IViewComponentResult Invoke()
        {
           var cart = HttpContext.Session.Get<List<CartItemViewModel>>(ConstSetting.CART_KEY) ?? new List<CartItemViewModel>();

           return View("CartInfo", new CartInfoViewModel
           {
               Quantity = cart.Select(p => p.ProductId).Distinct().Count(),
               TotalPrice = cart.Sum(p => p.Total)
           });
        }
    }
}
