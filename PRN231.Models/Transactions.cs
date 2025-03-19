namespace PRN231.Models;

public partial class Transaction : BaseModel
{
    public int UserId { get; set; }

    public int? ReceiverId { get; set; }
    public string TransactionCode { get; set; }

    public decimal Amount { get; set; }

    public string? Message { get; set; }

    public string Type { get; set; }

    public User User { get; set; }

    public User? Receiver { get; set; }
}
