using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using TravelPlannerMVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace TravelPlannerMVC.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db;

        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

        #region REGISTER

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

        #endregion

        #region LOGIN

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            User user = db.Users.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();

            PasswordVerificationResult passwordResult = hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                model.Password
            );

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            List<Claim> userInfo = new List<Claim>();

            userInfo.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            userInfo.Add(new Claim(ClaimTypes.Name, user.Name));
            userInfo.Add(new Claim(ClaimTypes.Email, user.Email));
            userInfo.Add(new Claim(ClaimTypes.Role, user.Role));

            ClaimsIdentity userIdentity = new ClaimsIdentity(
                userInfo,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal
            );

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}