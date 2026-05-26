using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMVC.ViewModels
{
    public class TravelRequestCreateViewModel
    {
        [Required]
        public string StartCity { get; set; } = string.Empty;

        [Required]
        public string EndCity { get; set; } = string.Empty;

        [Required]
        public DateTime PreferredDate { get; set; }
    }
}