using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class ResponseUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        public string Avatar { get; set; }

        public string Gender { get; set; }

        public string Status { get; set; }
        public List<BookingUser>? BookingUsers { get; set; }
        public List<Credential>? Credentials { get; set; }
        public List<Post> Posts { get; set; }
    }
}
