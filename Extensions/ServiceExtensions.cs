using TravelPlannerMVC.Services;

namespace TravelPlannerMVC.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<BookingService>();
            services.AddScoped<TravelRequestService>();
        }
    }
}