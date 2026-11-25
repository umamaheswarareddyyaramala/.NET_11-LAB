using System.ComponentModel.DataAnnotations;

namespace SmartCampusPortal.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required, Display(Name = "Student Name")]
        public string StudentName { get; set; } = "";

        [Required]
        public string Category { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}