using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class LevelDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "LevelName is required")]
        [RegularExpression(@"^[a-zA-Z\sÀ-ỹ0-9]{1,50}$", ErrorMessage = "LevelName must contains letters and cannot contain special characters")]
        public string LevelName { get; set; }
        public string Status { get; set; }
    }
}
