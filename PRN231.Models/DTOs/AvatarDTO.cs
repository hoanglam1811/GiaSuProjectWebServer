using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class AvatarDTO
    {
        public int UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
