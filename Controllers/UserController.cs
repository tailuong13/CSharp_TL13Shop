using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
            return View();
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
        public IActionResult Profile(int userId)
		{
			var user = db.Users.SingleOrDefault(u => u.UserId == userId);

			if (user == null)
			{
				return NotFound();
			}

			var model = new ProfileViewModel
			{
				UserName = user.UserName,
				FullName = user.FullName,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				Address = user.Address
			};

			return View(model);
		}
		[Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
