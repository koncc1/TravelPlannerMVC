using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMVC.ViewModels
{
    public class BookingCreateViewModel
    {
        public int TravelRouteId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Seats count must be from 1 to 100")]
        public int SeatsCount { get; set; }
    }
}