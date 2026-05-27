using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using TravelPlannerMVC.ViewModels;
using TravelPlannerMVC.DTO;


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

            var dto = new BookingCreateDto
            {
                UserId = userId,
                TravelRouteId = model.TravelRouteId,
                SeatsCount = model.SeatsCount
            };

            Booking booking = new Booking
            {
                UserId = dto.UserId,
                TravelRouteId = dto.TravelRouteId,
                SeatsCount = dto.SeatsCount,
                Status = "Confirmed"
            };

            route.AvailableSeats = route.AvailableSeats - dto.SeatsCount;

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
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            var booking = _context.Bookings.Find(id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.UserId != userId)
            {
                return Forbid();
            }

            if (booking.Status != "Cancelled")
            {
                var route = _context.TravelRoutes.Find(booking.TravelRouteId);

                if (route != null)
                {
                    route.AvailableSeats = route.AvailableSeats + booking.SeatsCount;
                }

                booking.Status = "Cancelled";

                _context.SaveChanges();
            }

            return RedirectToAction("MyBookings");
        }
    }

}