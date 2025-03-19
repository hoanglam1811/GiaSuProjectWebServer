namespace PRN231.Models.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string DayOfWeek { get; set; }

        public TimeOnly Duration { get; set; }
        
        public TimeOnly StartTime { get; set; }

        public string Status { get; set; }
    }
}