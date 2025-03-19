using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }

        [NotMapped]
        public List<string> ImageUrlList
        {
            get => string.IsNullOrEmpty(ImageUrl) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(ImageUrl);
            set => ImageUrl = JsonConvert.SerializeObject(value);
        }
        public List<PostRatingDTO> Ratings { get; set; }
    }
}
