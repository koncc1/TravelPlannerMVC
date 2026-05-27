using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using TravelPlannerMVC.ViewModels;

namespace TravelPlannerMVC.Controllers
{
    /// <summary>
    /// Controller for managing travel requests (create, view, admin status changes)
    /// </summary>
    [Authorize]
    public class TravelRequestsController : Controller
    {
        #region Fields & Constructor

        private readonly ApplicationDbContext _context;

        public TravelRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin - Requests List

        /// <summary>
        /// Displays all travel requests (Admin/Manager only)
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            var requests = _context.TravelRequests.ToList();
            return View(requests);
        }

        #endregion

        #region Create Request - GET

        /// <summary>
        /// Shows form for creating a new travel request
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        #endregion

        #region Create Request - POST

        /// <summary>
        /// Handles creation of travel request
        /// </summary>
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

        #endregion

        #region User Requests

        /// <summary>
        /// Shows requests of currently logged-in user
        /// </summary>
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

        #endregion

        #region Admin - Change Status

        /// <summary>
        /// Changes status of a travel request (Admin/Manager only)
        /// </summary>
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

        #endregion
    }
}