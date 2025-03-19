namespace PRN231.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        public string Avatar { get; set; }

        public string Gender { get; set; }

        public string Status { get; set; }

        public string? Otp { get; set; }
        public DateTime? OtpExpiration { get; set; }

    }
}
