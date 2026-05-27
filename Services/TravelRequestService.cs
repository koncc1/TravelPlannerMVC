using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;

namespace TravelPlannerMVC.Services
{
    /// <summary>
    /// Service responsible for travel request business logic
    /// </summary>
    public class TravelRequestService
    {
        #region Fields & Constructor

        private readonly ApplicationDbContext _context;

        public TravelRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Data Access

        /// <summary>
        /// Gets travel request by ID
        /// </summary>
        public TravelRequest? GetById(int id)
        {
            return _context.TravelRequests.Find(id);
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Changes status of a travel request and saves changes
        /// </summary>
        public void ChangeStatus(TravelRequest request, string status)
        {
            request.Status = status;
            _context.SaveChanges();
        }

        #endregion
    }
}