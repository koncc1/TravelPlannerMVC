namespace TravelPlannerMVC.DTO
{
    public class TravelRequestCreateDto
    {
        public int UserId { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime PreferredDate { get; set; }
    }
}