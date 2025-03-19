namespace PRN231.Models.DTOs
{
    public class TransactionDTO
    {
        public int UserId { get; set; }

        public int? ReceiverId { get; set; }

        public string TransactionCode { get; set; }

        public decimal Amount { get; set; }

        public string? Message { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}
