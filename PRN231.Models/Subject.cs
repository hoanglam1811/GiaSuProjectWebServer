namespace PRN231.Models;

public partial class Subject : BaseModel
{
    public string Name { get; set; }

    public List<Booking> Bookings { get; set; }

    public List<Credential> Credentials { get; set; }
}
