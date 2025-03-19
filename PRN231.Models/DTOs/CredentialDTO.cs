using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class CredentialDTO
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        [Required(ErrorMessage = "SubjectId is required")]
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z\sÀ-ỹ]$", ErrorMessage = "Name must be characters, without numbers and special characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [MaxLength(100, ErrorMessage = "Type cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z\sÀ-ỹ]$", ErrorMessage = "Type must be characters, without numbers and special characters")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Image { get; set; }

        public string Status { get; set; }

    }
}
