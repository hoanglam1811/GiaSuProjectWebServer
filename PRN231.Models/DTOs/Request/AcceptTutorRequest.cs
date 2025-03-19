using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs.Request
{
    public class AcceptTutorRequest
    {
        public int BookingId { get; set; }
        public int TutorId { get; set; }
    }
}
