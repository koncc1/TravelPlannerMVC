using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using TravelPlannerMVC.ViewModels;

namespace TravelPlannerMVC.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int routeId)
        {
            var route = _context.TravelRoutes.Find(routeId);

            if (route == null)
            {
                return NotFound();
            }

            ViewBag.Route = route;

            BookingCreateViewModel model = new BookingCreateViewModel
            {
                TravelRouteId = routeId,
                SeatsCount = 1
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(BookingCreateViewModel model)
        {
            var route = _context.TravelRoutes.Find(model.TravelRouteId);

            if (route == null)
            {
                return NotFound();
            }

            ViewBag.Route = route;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.SeatsCount > route.AvailableSeats)
            {
                ModelState.AddModelError("SeatsCount", "Not enough available seats");
                return View(model);
            }

            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            Booking booking = new Booking
            {
                UserId = userId,
                TravelRouteId = model.TravelRouteId,
                SeatsCount = model.SeatsCount,
                Status = "Confirmed"
            };

            route.AvailableSeats = route.AvailableSeats - model.SeatsCount;

            _context.Bookings.Add(booking);
            _context.TravelRoutes.Update(route);
            _context.SaveChanges();

            return RedirectToAction("MyBookings");
        }

        public IActionResult MyBookings()
        {
            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            var bookings = _context.Bookings
                .Include(b => b.TravelRoute)
                .Where(b => b.UserId == userId)
                .ToList();

            return View(bookings);
        }
    }
}