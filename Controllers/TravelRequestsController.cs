using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using TravelPlannerMVC.ViewModels;

namespace TravelPlannerMVC.Controllers
{
    [Authorize]
    public class TravelRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            var requests = _context.TravelRequests.ToList();

            return View(requests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TravelRequestCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdText == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int userId = int.Parse(userIdText);

                TravelRequest request = new TravelRequest
                {
                    UserId = userId,
                    StartCity = model.StartCity,
                    EndCity = model.EndCity,
                    PreferredDate = model.PreferredDate,
                    Status = "Pending"
                };

                _context.TravelRequests.Add(request);
                _context.SaveChanges();

                return RedirectToAction("MyRequests");
            }

            return View(model);
        }

        public IActionResult MyRequests()
        {
            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            var requests = _context.TravelRequests
                .Where(r => r.UserId == userId)
                .ToList();

            return View(requests);
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            var request = _context.TravelRequests.Find(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = status;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}