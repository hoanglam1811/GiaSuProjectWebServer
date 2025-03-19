namespace PRN231.Models.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }

        public string LevelName { get; set; }

        public decimal? PricePerSlot { get; set; }
        public DateTime StartDate { get; set; }

        public int NumOfSlots { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Description { get; set; }

        public IEnumerable<ScheduleDTO> Schedules { get; set; }

        public IEnumerable<BookingUserDTO> BookingUsers { get; set; }
        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
