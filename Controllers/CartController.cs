using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TL13Shop.Data;
using TL13Shop.Helpers;
using TL13Shop.Models;

namespace TL13Shop.Controllers
{
	public class CartController : Controller
	{
		private readonly Tl13shopContext db;

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

		[Authorize]
		[HttpGet]
		public IActionResult CheckOut()
		{
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = db.Users.SingleOrDefault(u => u.UserId == int.Parse(userId));
			var cartItem = Cart;

			if (cartItem.Count == 0)
			{
				return Redirect("/");
			}
            
			var data = new CheckoutViewModel
            {
                CustomerName = user.FullName,
                CustomerAddress = user.Address,
                CustomerPhone = user.PhoneNumber,
                CartItems = cartItem
            };

			return View(data);
		}

		[Authorize]
		[HttpPost]
		public IActionResult CheckOut(CheckoutViewModel model)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
			var user = db.Users.SingleOrDefault(u => u.UserId == int.Parse(userId));
			if (ModelState.IsValid)
			{
				var order = new Order
				{
					UserId = int.Parse(userId),
					CustomerName = model.CustomerName,
					CustomerPhone = model.CustomerPhone,
					CustomerAddress = model.CustomerAddress,
					PaymentMethod = "COD",
					Note = model.Note,
					StatusId = 1,
					OrderDate = DateTime.Now,
					IsCancel = false
				};

				db.Database.BeginTransaction();
				try
				{
					
					db.Add(order);
					db.SaveChanges();

					var orderDetail = new List<OrderDetail>();
					foreach (var item in Cart)
					{
						orderDetail.Add(new OrderDetail
						{
							OrderId = order.OrderId,
							ProductId = item.ProductId,
							Amount = item.Quantity,
							Total = (double)(item.Price * item.Quantity),
							Discount = 0
						});

						var product = db.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
						if (product != null)
						{
							product.Quantity -= item.Quantity;
							product.Sold += item.Quantity;
						}
					}
					db.OrderDetails.AddRange(orderDetail);
					db.SaveChanges();

					db.Database.CommitTransaction();

					var cart = Cart;
					cart.Clear();
					HttpContext.Session.Set(ConstSetting.CART_KEY, cart);

					return View("Success");

				}
				catch
				{
					db.Database.RollbackTransaction();
				}
			}
			return View();
		}
	}
}
