using Microsoft.AspNetCore.Identity;

namespace PRN231.Models;

public partial class User : IdentityUser<int> 
{
    public string Address { get; set; }

    public string Avatar { get; set; }

    public string Gender { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; }

    public decimal Credit { get; set; }

    public List<Credential>? Credentials { get; set; }

    public List<Feedback>? StudentFeedbacks { get; set; }

    public List<Feedback>? TutorFeedbacks { get; set; }

    public List<BookingUser>? BookingUsers { get; set; }
    public List<Post>? Posts { get; set; }

    public List<Transaction>? SentTransactions { get; set; }
    public List<Transaction>? ReceivedTransactions { get; set; }
}
