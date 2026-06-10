using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareCRM.Models
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Foreign Key Linking to Patient
        [Required(ErrorMessage = "Patient selection is required.")]
        public Guid PatientId { get; set; }

        // Navigation Property for Entity Framework
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }

        [Required(ErrorMessage = "Appointment date and time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Reason for appointment is required.")]
        [StringLength(250, ErrorMessage = "Reason cannot exceed 250 characters.")]
        public string Reason { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Canceled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}