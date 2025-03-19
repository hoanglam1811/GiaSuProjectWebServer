using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class PostRatingDTO
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int Rating { get; set; }
        public string Status { get; set; }
    }
}
