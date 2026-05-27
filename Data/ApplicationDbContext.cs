using Microsoft.EntityFrameworkCore;
using TravelPlannerMVC.Models;

namespace TravelPlannerMVC.Data
{
    /// <summary>
    /// Main EF Core database context for TravelPlanner application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructor

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region DbSets - Entities

        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Travel routes table
        /// </summary>
        public DbSet<TravelRoute> TravelRoutes { get; set; }

        /// <summary>
        /// Bookings table
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Travel requests table
        /// </summary>
        public DbSet<TravelRequest> TravelRequests { get; set; }

        #endregion
    }
}