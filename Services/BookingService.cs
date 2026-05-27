using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace TravelPlannerMVC.Services
{
    public class BookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public TravelRoute? GetRoute(int id)
        {
            return _context.TravelRoutes.Find(id);
        }

        public void CreateBooking(Booking booking, TravelRoute route)
        {
            route.AvailableSeats -= booking.SeatsCount;

            _context.Bookings.Add(booking);
            _context.TravelRoutes.Update(route);

            _context.SaveChanges();
        }
    }
}