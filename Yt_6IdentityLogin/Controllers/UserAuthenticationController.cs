using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yt_6IdentityLogin.Models.DTO;
using Yt_6IdentityLogin.Repositories.Abstract;

namespace Yt_6IdentityLogin.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        public IActionResult Registeration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registeration(RegisterationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Role = "user";
            var result = await _userAuthenticationService.RegisterationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Login));
            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                RedirectToAction("Display", "Dashboard");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await _userAuthenticationService.LoginAsync(model);
                if (result.StatusCode == 1)
                {
                    HttpContext.Session.SetString("Username", model.Username);
                    return RedirectToAction("Display", "Dashboard");
                }
                else
                {
                    TempData["msg"] = result.Message;
                    return RedirectToAction(nameof(Login));
                }
            }
            return View(model);
            
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _userAuthenticationService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //public async Task<IActionResult> reg()
        //{
        //    var model = new RegisterationModel
        //    {
        //        UserName = "admin1",
        //        Name = "Adhu",
        //        Email = "adhu@gmail.com",
        //        Password = "Admin1@123#",
        //    };
        //    model.Role = "admin";
        //    var result = await _userAuthenticationService.RegisterationAsync(model);
        //    return Ok(result);
        //}
    }
}
