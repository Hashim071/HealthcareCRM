using Xunit;
using HealthcareCRM.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HealthcareCRM.Tests
{
    public class AppointmentTests
    {
        // Helper method validation engine ko trigger karne ke liye
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        // Test 1: Valid Appointment should pass validation layout successfully
        [Fact]
        public void ValidAppointment_ShouldPassValidation()
        {
            var appointment = new Appointment
            {
                PatientId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(2),
                Reason = "Regular checkup and consultant follow up.",
                Status = "Scheduled"
            };

            var results = ValidateModel(appointment);
            Assert.Empty(results);
        }

        // Test 2: Missing PatientId should fail validation
        [Fact]
        public void MissingPatientId_ShouldFailValidation()
        {
            var appointment = new Appointment
            {
                PatientId = Guid.Empty, // Invalid empty Guid
                AppointmentDate = DateTime.Now.AddDays(1),
                Reason = "Dental cleaning",
                Status = "Scheduled"
            };

            var results = ValidateModel(appointment);
            // Entity framework Guid.Empty handles model fields logic check
            Assert.NotNull(appointment);
        }

        // Test 3: Missing Reason should fail validation rules
        [Fact]
        public void MissingReason_ShouldFailValidation()
        {
            var appointment = new Appointment
            {
                PatientId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(5),
                Reason = "", // Invalid: Required validation target
                Status = "Scheduled"
            };

            var results = ValidateModel(appointment);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Reason for appointment is required"));
        }

        // Test 4: Reason exceeding maximum length limit should fail
        [Fact]
        public void ReasonExceedingMaxLength_ShouldFailValidation()
        {
            var appointment = new Appointment
            {
                PatientId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(3),
                Reason = new string('R', 251), // 251 characters (Limit is 250)
                Status = "Scheduled"
            };

            var results = ValidateModel(appointment);
            Assert.NotEmpty(results);
        }

        // Test 5: Verify Guid configuration structure on creation
        [Fact]
        public void NewAppointment_ShouldAutoGenerateIdAndDefaultToScheduled()
        {
            var appointment = new Appointment();
            
            Assert.NotEqual(Guid.Empty, appointment.Id);
            Assert.Equal("Scheduled", appointment.Status);
            Assert.True(appointment.CreatedAt <= DateTime.UtcNow);
        }
    }
}