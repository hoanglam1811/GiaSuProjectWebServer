namespace PRN231.Models.DTOs.Request
{
    public class CreatePaymentRequest
    {
        public long Amount { get; set; }
        public string OrderInfo { get; set; }
    }

    public class CreatePaymentUserRequest
    {
        public int UserId { get; set; }
        public long Amount { get; set; }
        public string OrderInfo { get; set; }
    }

    public class TransferPaymentUserRequest
    {
        public int UserId { get; set; }
        public int ReceiverId { get; set; }
        public long Amount { get; set; }
        public string OrderInfo { get; set; }
    }
}
