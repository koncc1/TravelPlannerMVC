using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMVC.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TravelRouteId { get; set; }

        [Required]
        [Range(1, 100)]
        public int SeatsCount { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public User? User { get; set; }

        public TravelRoute? TravelRoute { get; set; }

    }
}