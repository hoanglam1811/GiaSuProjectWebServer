using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs.Request
{
    public class CancelApplicationRequest
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
    }
}
