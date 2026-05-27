namespace TravelPlannerMVC.DTO
{
    public class BookingCreateDto
    {
        public int TravelRouteId { get; set; }
        public int SeatsCount { get; set; }
        public int UserId { get; set; }
    }
}