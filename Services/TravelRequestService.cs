using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;

namespace TravelPlannerMVC.Services
{
    public class TravelRequestService
    {
        private readonly ApplicationDbContext _context;

        public TravelRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public TravelRequest? GetById(int id)
        {
            return _context.TravelRequests.Find(id);
        }

        public void ChangeStatus(TravelRequest request, string status)
        {
            request.Status = status;
            _context.SaveChanges();
        }
    }
}