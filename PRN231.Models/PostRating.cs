using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models
{
    public class PostRating : BaseModel
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int Rating { get; set; } 
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
