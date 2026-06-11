using Xunit;
using HealthcareCRM.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HealthcareCRM.Tests
{
    public class DoctorTests
    {
        // Helper method validation engine ko target karne ke liye
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        // Test 1: Valid Doctor profile should pass validation rules easily
        [Fact]
        public void ValidDoctor_ShouldPassValidation()
        {
            var doctor = new Doctor
            {
                Name = "Dr. Shafi Malik",
                Specialization = "Neurology",
                Email = "shafi.malik@hospital.com",
                Phone = "+923001234567",
                RoomNumber = "Cabin-102"
            };

            var results = ValidateModel(doctor);
            Assert.Empty(results);
        }

        // Test 2: Missing Name should fail model state validation rules
        [Fact]
        public void MissingName_ShouldFailValidation()
        {
            var doctor = new Doctor
            {
                Name = "", // Invalid empty name string
                Specialization = "Cardiology",
                Email = "dr.khan@hospital.com",
                Phone = "+923339876543",
                RoomNumber = "Room-12"
            };

            var results = ValidateModel(doctor);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Doctor Name is required"));
        }

        // Test 3: Invalid Email address format should trigger error
        [Fact]
        public void InvalidEmailFormat_ShouldFailValidation()
        {
            var doctor = new Doctor
            {
                Name = "Dr. Amna",
                Specialization = "Pediatrics",
                Email = "invalid-email-format", // Missing @ and domain extensions
                Phone = "+923214567890",
                RoomNumber = "Room-45"
            };

            var results = ValidateModel(doctor);
            Assert.NotEmpty(results);
        }

        // Test 4: Name exceeding 100 character threshold should fail validation
        [Fact]
        public void NameExceedingMaxLength_ShouldFailValidation()
        {
            var doctor = new Doctor
            {
                Name = new string('D', 101), // 101 characters (Max limit allowed is 100)
                Specialization = "Dermatology",
                Email = "dr.d@hospital.com",
                Phone = "+923001112223",
                RoomNumber = "Room-01"
            };

            var results = ValidateModel(doctor);
            Assert.NotEmpty(results);
        }

        // Test 5: Verify automatic configurations upon model object instantiation
        [Fact]
        public void NewDoctor_ShouldAutoGenerateGuidIdOnCreation()
        {
            var doctor = new Doctor();
            
            Assert.NotEqual(Guid.Empty, doctor.Id);
            Assert.True(doctor.CreatedAt <= DateTime.UtcNow);
        }
    }
}