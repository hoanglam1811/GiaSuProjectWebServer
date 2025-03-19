namespace PRN231.Models;

public partial class Feedback : BaseModel
{
    public int TutorId { get; set; }

    public int StudentId { get; set; }
    public string SubjectName { get; set; }
    public string LevelName { get; set; }

    public string Content { get; set; }

    public int Rating { get; set; }

    public User Tutor { get; set; }

    public User Student { get; set; }
    public int BookingId { get; set; }
}
