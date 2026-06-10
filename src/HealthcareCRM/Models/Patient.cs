using System;
using System.ComponentModel.DataAnnotations;

namespace HealthcareCRM.Models
{
    public class Patient
    {
        // 1. Unique ID (Guid)
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // 2. Full Name (Validation ke sath)
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        // 3. Email
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // 4. Phone Number
        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        // 5. Date of Birth
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        // 6. Gender
        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        // 7. Address
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        // 8. Created At Timestamp
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // One Patient can have Many Appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}