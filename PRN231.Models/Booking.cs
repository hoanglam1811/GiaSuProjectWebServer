namespace PRN231.Models;

public partial class Booking : BaseModel
{
    public int SubjectId { get; set; }

    public int LevelId { get; set; }

    public decimal? PricePerSlot { get; set; }

    public DateTime StartDate { get; set; }

    public int NumOfSlots { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Description { get; set; }
    public Level Level { get; set; }

    public Subject Subject { get; set; }

    public List<Schedule> Schedules { get; set; }

    public List<BookingUser> BookingUsers { get; set; }

}
