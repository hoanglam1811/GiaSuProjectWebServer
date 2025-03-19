using System.ComponentModel.DataAnnotations;

namespace PRN231.Models.DTOs
{
    public class UserInfoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Receiver Name is required")]
        [RegularExpression(@"^[a-zA-Z\sÀ-ỹ]+$", ErrorMessage = "Receiver Name must contain only letters and cannot contain special characters or numbers")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{10,}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Avatar is required")]
        public string Avatar { get; set; }
    }
}
