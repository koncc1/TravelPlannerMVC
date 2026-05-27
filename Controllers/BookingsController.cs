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
        #region Fields & Constructor

        // Database context for working with EF Core
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Create Booking - GET

        // Displays booking creation page
        [HttpGet]
        public IActionResult Create(int routeId)
        {
            // Searching selected route in database
            var route = _context.TravelRoutes.Find(routeId);

            // Route existence validation
            if (route == null)
            {
                return NotFound();
            }

            // Sending route info to View
            ViewBag.Route = route;

            // Preparing default form values
            BookingCreateViewModel model = new BookingCreateViewModel
            {
                TravelRouteId = routeId,
                SeatsCount = 1
            };

            return View(model);
        }

        #endregion

        #region Create Booking - POST

        // Handles booking form submission
        [HttpPost]
        public IActionResult Create(BookingCreateViewModel model)
        {
            // Finding selected route
            var route = _context.TravelRoutes.Find(model.TravelRouteId);

            if (route == null)
            {
                return NotFound();
            }

            ViewBag.Route = route;

            // Validation of form fields
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Business Rule:
            // User cannot reserve more seats than available
            if (model.SeatsCount > route.AvailableSeats)
            {
                ModelState.AddModelError(
                    "SeatsCount",
                    "Not enough available seats"
                );

                return View(model);
            }

            // Extracting currently logged-in user ID from authentication token
            string? userIdText =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Safety check in case token is invalid
            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            // Mapping ViewModel -> DTO
            // DTO is used to separate UI layer from business logic
            var dto = new BookingCreateDto
            {
                UserId = userId,
                TravelRouteId = model.TravelRouteId,
                SeatsCount = model.SeatsCount
            };

            // Mapping DTO -> Domain Model
            Booking booking = new Booking
            {
                UserId = dto.UserId,
                TravelRouteId = dto.TravelRouteId,
                SeatsCount = dto.SeatsCount,
                Status = "Confirmed"
            };

            // Updating available seats after successful reservation
            route.AvailableSeats =
                route.AvailableSeats - dto.SeatsCount;

            // Saving booking into database
            _context.Bookings.Add(booking);

            // Updating modified route entity
            _context.TravelRoutes.Update(route);

            // Committing all changes to database
            _context.SaveChanges();

            return RedirectToAction("MyBookings");
        }

        #endregion

        #region Get User Bookings

        // Displays all bookings created by logged-in user
        public IActionResult MyBookings()
        {
            // Reading user ID from Claims
            string? userIdText =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            // Eager Loading:
            // .Include() loads related TravelRoute entity
            // in the same SQL query to improve efficiency
            var bookings = _context.Bookings
                .Include(b => b.TravelRoute)
                .Where(b => b.UserId == userId)
                .ToList();

            return View(bookings);
        }

        #endregion

        #region Cancel Booking

        // Cancels existing booking
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            // Extracting logged-in user ID
            string? userIdText =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdText == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdText);

            // Finding booking by ID
            var booking = _context.Bookings.Find(id);

            if (booking == null)
            {
                return NotFound();
            }

            // Security Protection:
            // Preventing users from cancelling чужі bookings
            if (booking.UserId != userId)
            {
                return Forbid();
            }

            // Preventing duplicate cancellation
            if (booking.Status != "Cancelled")
            {
                // Finding related travel route
                var route =
                    _context.TravelRoutes.Find(booking.TravelRouteId);

                if (route != null)
                {
                    // Returning reserved seats back to route capacity
                    route.AvailableSeats =
                        route.AvailableSeats + booking.SeatsCount;
                }

                // Updating booking status
                booking.Status = "Cancelled";

                // Saving all changes
                _context.SaveChanges();
            }

            return RedirectToAction("MyBookings");
        }

        #endregion
    }
}