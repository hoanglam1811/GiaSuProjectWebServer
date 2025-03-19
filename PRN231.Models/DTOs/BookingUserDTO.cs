using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class BookingUserDTO
    {
        public int UserId { get; set; }

        public int BookingId { get; set; }

        public string Status { get; set; }

        public string? Description { get; set; }

        public string? Role { get; set; }
    }
}
