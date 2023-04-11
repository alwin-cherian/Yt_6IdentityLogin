using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Yt_6IdentityLogin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Display()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewData["Username"] = username;
            return View();
        }
    }
}
