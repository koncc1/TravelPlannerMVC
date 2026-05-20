using Microsoft.EntityFrameworkCore;
using TravelPlannerMVC.Models;

namespace TravelPlannerMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<TravelRoute> TravelRoutes { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<TravelRequest> TravelRequests { get; set; }
    }
}