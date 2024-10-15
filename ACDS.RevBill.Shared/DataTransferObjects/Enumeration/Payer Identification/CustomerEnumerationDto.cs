using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
    public class CustomerEnumerationDto
    {
        [Required(ErrorMessage = "Type is a required field.")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Title is a required field.")]
        public string? Title { get; set; }

        public string? Hash { get; set; }

        public string? ClientId { get; set; }

        [Required(ErrorMessage = "Sex is a required field.")]
        public string? Sex { get; set; }

        [Required(ErrorMessage = "Lastname is a required field.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Firstname is a required field.")]
        public string? FirstName { get; set; }

        public string? OtherName { get; set; }

        [Required(ErrorMessage = "MaritalStatus is a required field.")]
        public string? MaritalStatus { get; set; }

        [Required(ErrorMessage = "DateOfBirth is a required field.")]
        public string? DateOfBirth { get; set; }

        [Required(ErrorMessage = " PhoneNumber is a required field.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string? Address { get; set; }

        public string? State { get; set; }
    }
}

