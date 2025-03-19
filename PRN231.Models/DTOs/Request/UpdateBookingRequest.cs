namespace PRN231.Models.DTOs.Request
{
    public class UpdateBookingRequest
    {
        public int BookingId { get; set; }
        public int SubjectId { get; set; }
        public int LevelId { get; set; }
        public decimal PricePerSlot { get; set; }
        public DateTime StartDate { get; set; }
        public int NumOfSlots { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

    public class UpdateBookingStatusRequest
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
    }
}
