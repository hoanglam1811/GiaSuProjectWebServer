using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models
{
    public partial class Post : BaseModel
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public User User { get; set; }
        public List<PostRating> Ratings { get; set; } = new List<PostRating>();
    }
}
