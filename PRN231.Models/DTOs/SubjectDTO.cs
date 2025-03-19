using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z\sÀ-ỹ]$", ErrorMessage = "Name must contain only letters and cannot contain special characters or numbers")]
        public string Name { get; set; }

        public string Status { get; set; }

    }
}
