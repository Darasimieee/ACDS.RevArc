using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class CustomerEnumerationBVNDto
	{
        [Required(ErrorMessage = "Type is a required field.")]
        public string? Type { get; init; }

        [Required(ErrorMessage = "Title is a required field.")]
        public string? Title { get; init; }

        public string? Hash { get; set; }

        public string? ClientId { get; set; }

        [Required(ErrorMessage = "Sex is a required field.")]
        public string? Sex { get; init; }

        [Required(ErrorMessage = "Lastname is a required field.")]
        public string? LastName { get; init; }

        [Required(ErrorMessage = "Firstname is a required field.")]
        public string? FirstName { get; init; }

        public string? MiddleName { get; init; }

        [Required(ErrorMessage = "MaritalStatus is a required field.")]
        public string? MaritalStatus { get; init; }

        [Required(ErrorMessage = "DateOfBirth is a required field.")]
        public string? DateOfBirth { get; init; }

        [Required(ErrorMessage = " PhoneNumber is a required field.")]
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "Email is a required field.")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string? Address { get; init; }

        [Required(ErrorMessage = "BVNNumber is a required field.")]
        public string? BVNNumber { get; set; }

        [Required(ErrorMessage = "UserPID is a required field.")]
        public string? UserPID { get; init; }

        public string? State { get; set; }
    }
}

