namespace PRN231.Models;

public partial class Schedule : BaseModel
{
    public int BookingId { get; set; }

    public string DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly Duration { get; set; }

    public Booking Booking { get; set; }
}
