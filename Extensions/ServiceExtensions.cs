using TravelPlannerMVC.Services;

namespace TravelPlannerMVC.Extensions
{
    /// <summary>
    /// Extension methods for registering application services into Dependency Injection container
    /// </summary>
    public static class ServiceExtensions
    {
        #region Service Registration

        /// <summary>
        /// Registers custom application services into IServiceCollection
        /// </summary>
        public static void AddAppServices(this IServiceCollection services)
        {
            // Booking business logic service
            services.AddScoped<BookingService>();

            // Travel request business logic service
            services.AddScoped<TravelRequestService>();
        }

        #endregion
    }
}