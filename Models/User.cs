using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User";

        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public List<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();

    }
}
