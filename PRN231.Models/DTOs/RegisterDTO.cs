using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class RegisterDTO
    {
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        public required string Name { get; set; }
        [MinLength(3, ErrorMessage = "Email must be at least 3 characters")]
        public required string Email { get; set; }
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters")]
        public required string Password { get; set; }
    }
}
