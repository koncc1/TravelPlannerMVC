using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlannerMVC.Areas.Admin.Models;
using TravelPlannerMVC.Data;

namespace TravelPlannerMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel
            {
                UsersCount = _context.Users.Count(),
                RoutesCount = _context.TravelRoutes.Count(),
                BookingsCount = _context.Bookings.Count(),
                RequestsCount = _context.TravelRequests.Count()
            };

            return View(model);
        }
    }
}