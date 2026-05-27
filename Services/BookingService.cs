using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace TravelPlannerMVC.Services
{
    /// <summary>
    /// Service responsible for booking business logic and database operations
    /// </summary>
    public class BookingService
    {
        #region Fields & Constructor

        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Get Data

        /// <summary>
        /// Retrieves travel route by ID
        /// </summary>
        public TravelRoute? GetRoute(int id)
        {
            return _context.TravelRoutes.Find(id);
        }

        #endregion

        #region Booking Logic

        /// <summary>
        /// Creates a booking and updates available seats for the route
        /// </summary>
        public void CreateBooking(Booking booking, TravelRoute route)
        {
            // Business rule: decrease available seats
            route.AvailableSeats -= booking.SeatsCount;

            // Persist booking
            _context.Bookings.Add(booking);

            // Update route state
            _context.TravelRoutes.Update(route);

            // Commit transaction
            _context.SaveChanges();
        }

        #endregion
    }
}