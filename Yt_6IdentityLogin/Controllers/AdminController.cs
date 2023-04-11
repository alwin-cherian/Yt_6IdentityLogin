using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yt_6IdentityLogin.Models.Domain;
using Yt_6IdentityLogin.Models.DTO;
using Yt_6IdentityLogin.Repositories.Abstract;

namespace Yt_6IdentityLogin.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AdminController(DatabaseContext db, UserManager<ApplicationUser> userManager , IUserAuthenticationService userAuthenticationService)
        {
            _db = db;
            _userManager = userManager;
            _userAuthenticationService = userAuthenticationService;
        }

        public IActionResult Display()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var UserList = await _db.Users.ToListAsync();
            return View(UserList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Role = "user";
            var result = await _userAuthenticationService.RegisterationAsync(model);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(ApplicationUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Update(user);
        //        await _db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(user);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = entry.GetDatabaseValues();
                    if (databaseValues == null)
                    {
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was deleted by another user.");
                        return RedirectToAction("Index");
                    }

                    var databaseUser = (ApplicationUser)databaseValues.ToObject();
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled and the current values in the database have been displayed for your reference.");

                    // Reload the entity from the database
                    _db.Entry(user).Reload();
                    ModelState.Remove("Id");
                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
