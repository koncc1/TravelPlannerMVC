using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMVC.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Required]
        public string StartCity { get; set; } = string.Empty;

        [Required]
        public string EndCity { get; set; } = string.Empty;

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 100)]
        public int AvailableSeats { get; set; }
    }
}
