using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMVC.Models
{
    public class TravelRequest
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string StartCity { get; set; }

        [Required]
        public string EndCity { get; set; }

        [Required]
        public DateTime PreferredDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public User User { get; set; }
    }
}
