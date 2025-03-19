namespace PRN231.Models.DTOs.Response
{
    public class CreatePaymentResponse
    {
        public string OrderId { get; set; }

        public string OrderInfo { get; set; }

        public string Amount { get; set; }

        public string PayDate { get; set; }
    }
}
