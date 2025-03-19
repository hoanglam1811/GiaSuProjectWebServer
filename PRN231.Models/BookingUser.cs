namespace PRN231.Models;

public partial class BookingUser : BaseModel
{
    public int UserId { get; set; }

    public int BookingId { get; set; }
    public string Status { get; set; }
    public string? Description { get; set; }

    public string? Role { get; set; }

    public User User { get; set; }

    public Booking Booking { get; set; }
}
