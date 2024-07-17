using Microsoft.AspNetCore.Mvc;
using TL13Shop.Data;
using TL13Shop.Helpers;
using TL13Shop.Models;

namespace TL13Shop.Controllers
{
	public class CartController : Controller
	{
		public readonly Tl13shopContext db;

		public CartController(Tl13shopContext context)
		{
			db = context;
		}

		public List<CartItemViewModel> Cart => HttpContext.Session.Get<List<CartItemViewModel>>(ConstSetting.CART_KEY) ?? new List<CartItemViewModel>();

		public IActionResult Index()
		{
			return View(Cart);
		}

		public IActionResult AddToCart(int productId, int quantity = 1)
		{
			var cart = Cart;
			var item = cart.SingleOrDefault(p => p.ProductId == productId);

			if (item == null)
			{
				var product = db.Products.SingleOrDefault(p => p.ProductId == productId);
				if (product == null)
				{
					TempData["Message"] = "Can't found product id " + productId.ToString();
					return Redirect("/404");
				}
				
				var productImages = db.ProductImages.AsQueryable();
				item = new CartItemViewModel
				{
					ProductId = product.ProductId,
					ProductName = product.ProductName,
					ProductImg = productImages.Where(i => i.ProductId == product.ProductId && i.DefaultImage == true).Select(i => i.ImageUrl).FirstOrDefault() ?? "",
					Quantity = quantity,
					Price = product.Price
				};
				cart.Add(item);
			}
			else
			{
				item.Quantity += quantity;
			}
			HttpContext.Session.Set(ConstSetting.CART_KEY, cart);
			return RedirectToAction("Index");
		}

		public IActionResult DeleteCart(int productId)
		{
			var cart = Cart;
			var item = cart.SingleOrDefault(p => p.ProductId == productId);

			if (item != null)
			{
				cart.Remove(item);
				HttpContext.Session.Set(ConstSetting.CART_KEY, cart);
			}
			return RedirectToAction("Index");
		}
	}
}
