using Xunit;
using HealthcareCRM.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

// Yeh attribute ensure karega ke test runner is single project ke andar tests ko dhund sake
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace HealthcareCRM.Tests
{
    public class PatientTests
    {
        // Helper method validations trigger karne ke liye
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        // Test 1: Valid Patient Model should pass validation
        [Fact]
        public void ValidPatient_ShouldPassValidation()
        {
            var patient = new Patient
            {
                FullName = "Hashim Sadiq",
                Email = "hashim@example.com",
                PhoneNumber = "+923001234567",
                DateOfBirth = DateTime.Now.AddYears(-25),
                Gender = "Male",
                Address = "I-8 Sector, Islamabad"
            };

            var results = ValidateModel(patient);
            Assert.Empty(results);
        }

        // Test 2: Empty FullName should fail validation
        [Fact]
        public void MissingFullName_ShouldFailValidation()
        {
            var patient = new Patient
            {
                FullName = "", // Invalid
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Gender = "Male",
                Address = "Rawalpindi"
            };

            var results = ValidateModel(patient);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Full Name is required"));
        }

        // Test 3: Invalid Email pattern should fail validation
        [Fact]
        public void InvalidEmailFormat_ShouldFailValidation()
        {
            var patient = new Patient
            {
                FullName = "Ali Khan",
                Email = "invalid-email-format", // Invalid
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Gender = "Male",
                Address = "Blue Area, Islamabad"
            };

            var results = ValidateModel(patient);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Invalid email address format"));
        }

        // Test 4: Name exceeding character limits should fail
        [Fact]
        public void NameExceedingMaxLength_ShouldFailValidation()
        {
            var patient = new Patient
            {
                FullName = new string('A', 101), // 101 chars (Limit is 100)
                Email = "ali@example.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-22),
                Gender = "Male",
                Address = "Gulberg Greens"
            };

            var results = ValidateModel(patient);
            Assert.NotEmpty(results);
        }

        // Test 5: Verify Guid Initialization
        [Fact]
        public void NewPatient_ShouldAutoGenerateGuidAndTimestamp()
        {
            var patient = new Patient();
            Assert.NotEqual(Guid.Empty, patient.Id);
            Assert.True(patient.CreatedAt <= DateTime.UtcNow);
        }
    }
}