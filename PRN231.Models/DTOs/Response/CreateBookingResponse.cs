namespace PRN231.Models.DTOs.Response
{
    public class CreateBookingResponse
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }

        public int LevelId { get; set; }

        public int UserId { get; set; }
        
        public DateTime StartDate { get; set; }
        public string Role { get; set; }

        public string Description { get; set; }

        public int NumOfSlots { get; set; }

        public decimal PricePerSlot { get; set; }
    }
}
