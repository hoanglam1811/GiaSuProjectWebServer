using PRN231.Models;

namespace PRN231.Models.DTOs
{
    public class FeedbackDTO
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int StudentId { get; set; }
        public string SubjectName { get; set; }
        public string LevelName { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }
        public int BookingId { get; set; }

        public string Status { get; set; }
    }
}
