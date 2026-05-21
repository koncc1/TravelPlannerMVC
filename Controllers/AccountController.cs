using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using TravelPlannerMVC.ViewModels;

namespace TravelPlannerMVC.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db;

        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            User existingUser = db.Users.FirstOrDefault(user => user.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists");
                return View(model);
            }

            User newUser = new User();

            newUser.Name = model.Name;
            newUser.Email = model.Email;
            newUser.Role = "User";

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.PasswordHash = hasher.HashPassword(newUser, model.Password);



            db.Users.Add(newUser);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}