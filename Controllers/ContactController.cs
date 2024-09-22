using Microsoft.AspNetCore.Mvc;

namespace TL13Shop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
