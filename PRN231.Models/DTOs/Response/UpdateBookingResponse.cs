namespace PRN231.Models.DTOs.Response
{
    public class UpdateBookingResponse
    {
        public int SubjectId { get; set; }
        public int LevelId { get; set; }
        public decimal PricePerSlot { get; set; }
        public DateTime StartDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public int NumOfSlots { get; set; }

    }
}
