using Microsoft.AspNetCore.Mvc;
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
					return View();
				}
				else
				{
					var user = new User
					{
						UserName = model.UserName,
						Password = model.Password,
						CreatedDate = DateTime.Now
					};
					var RandomKey = Util.GenerateRandomKey();
					user.Password = model.Password.ToMd5Hash(RandomKey);
					user.RoleId = 2;
					db.Users.Add(user);
					db.SaveChanges();
					return RedirectToAction("Index", "Home");
				}

			}
			return View();
		}
	}
}
