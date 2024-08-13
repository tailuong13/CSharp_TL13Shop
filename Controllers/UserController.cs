using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TL13Shop.Data;
using TL13Shop.Helpers;
using TL13Shop.Models;

namespace TL13Shop.Controllers
{
	public class UserController : Controller
	{
		public readonly Tl13shopContext db;

		public UserController(Tl13shopContext context)
		{
			db = context;
		}

		#region Register
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var existingUser = db.Users.SingleOrDefault(u => u.UserName == model.UserName);

				if (existingUser != null)
				{
					ModelState.AddModelError("UserName", "Username is already exist");
					return View(model);
				}
				else
				{
					try
					{
						var randomKey = Util.GenerateRandomKey();
						var hashedPassword = model.Password.ToMd5Hash(randomKey);

						var user = new User
						{
							UserName = model.UserName,
							Password = hashedPassword,
							RandomKey = randomKey,
							CreatedDate = DateTime.Now,
							ImageUrl = "UserImg/default.jpg",
							RoleId = 2
						};
						db.Add(user);
						db.SaveChanges();
						return RedirectToAction("Index", "Home");
					}
					catch (Exception e)
					{
						ModelState.AddModelError("", "An error occurred while registering the user. Please try again.");
					}
				}

			}
			return View(model);
		}
		#endregion

		#region Login
		[HttpGet]
		public IActionResult Login(string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			if (ModelState.IsValid)
			{
				var user = db.Users.SingleOrDefault(u => u.UserName == model.UserName);
				if (user == null)
				{
					ModelState.AddModelError("Error", "Username is not exist");
				}
				else
				{
					if (user.Password != model.Password.ToMd5Hash(user.RandomKey))
					{
						ModelState.AddModelError("Error", "Password is incorrect");
					}
					else
					{
						var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
							new Claim("RoleId", user.RoleId.ToString()),
							new Claim(ClaimTypes.Role, "Customer")
						};
						var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
						var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

						await HttpContext.SignInAsync(claimsPrincipal);

						if (Url.IsLocalUrl(ReturnUrl))
						{
							return Redirect(ReturnUrl);
						}
						else
						{
							return Redirect("/");
						}
					}
				}
			}
			return View();
		}
		#endregion
		[Authorize]
		[HttpGet]
		public IActionResult Profile(int userId)
		{
			var user = db.Users.SingleOrDefault(u => u.UserId == userId);

			if (user == null)
			{
				return NotFound();
			}

			var model = new ProfileViewModel
			{
				UserId = user.UserId,
				UserName = user.UserName,
				FullName = user.FullName,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				Address = user.Address,
				ImageUrl = user.ImageUrl
			};

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Profile(int userId, ProfileViewModel model, IFormFile Image)
		{
			if (ModelState.IsValid)
			{
				var user = db.Users.SingleOrDefault(u => u.UserId == userId);
				if (user == null)
				{
					ModelState.AddModelError("Error", "Username is not exist");
					return View(model);
				}
				else
				{
					try
					{
						user.FullName = model.FullName;
						user.PhoneNumber = model.PhoneNumber;
						user.Email = model.Email;
						user.Address = model.Address;

						if (Image != null)
						{
							var userIdFolder = user.UserId.ToString();
							user.ImageUrl = Util.UploadImage(Image, userIdFolder);
						}
						db.SaveChanges();
						return RedirectToAction("Profile", new { userId = user.UserId });
					}
					catch (Exception e)
					{
						ModelState.AddModelError("", "An error occurred while changing the infomation. Please try again.");
					}
				}
			}
			return View(model);
		}
		[Authorize]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public IActionResult Orders(int userId)
		{
			var orders = db.OrderDetails
				.Include(o => o.Product)
				.Where(o => o.Order.UserId == userId);
			var data = orders.Select(o => new OrdersByStatusViewModel
			{
				DetailId = o.DetailId,
				OrderId = o.OrderId,
				ProductId = o.ProductId,
				StatusId = o.Order.StatusId,
				ProductName = o.Product.ProductName,
				ProductImageUrl = o.Product.ProductImages.FirstOrDefault().ImageUrl,
				Quantity = o.Amount,
				Price = o.Product.Price,
				OrderDate = o.Order.OrderDate
			});
			return View(data);
		}

		public IActionResult OrdersByStatus(int? statusId, int userId)
		{
			var orders = db.OrderDetails
				.Include(o => o.Product)
				.Where(o => o.Order.UserId == userId);
			if (statusId != null)
			{
				orders = orders.Where(o => o.Order.StatusId == statusId);
			}

			var data = orders.Select(o => new OrdersByStatusViewModel
			{
				DetailId = o.DetailId,
				OrderId = o.OrderId,
				ProductId = o.ProductId,
				StatusId = o.Order.StatusId,
				ProductName = o.Product.ProductName,
				ProductImageUrl = o.Product.ProductImages.FirstOrDefault().ImageUrl,
				Quantity = o.Amount,
				Price = o.Product.Price,
				OrderDate = o.Order.OrderDate
			});

			return View(data);
		}

		public IActionResult OrderDetails(int detailId)
		{
			var order = db.OrderDetails
						.Include(o => o.Order)
						.Include(o => o.Product)
						.FirstOrDefault(o => o.DetailId == detailId);
			var data = new OrderDetailViewModel
			{
				DetailId = order.DetailId,
				OrderId = order.OrderId,
				CustomerName = order.Order.CustomerName,
				CustomerAddress = order.Order.CustomerAddress,
				CustomerPhone = order.Order.CustomerPhone,
				Price = order.Product.Price,
				Style = order.Product.Color
			};
			return PartialView("_OrderDetail", data);
		}
		[HttpPost]
		public IActionResult cancelOrder(int detailId)
		{
			var order = db.OrderDetails 
				.Include(o => o.Order)
				.FirstOrDefault(o => o.DetailId== detailId);
			if (order != null)
			{
				order.Order.StatusId = 4;
			}
			return View();
		}
	}
}
