using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserProfileUpdateDto
    {
        [Required(ErrorMessage = "Firstname is a required field.")]
        public string? Firstname { get; init; }

        public string? MiddleName { get; init; }

        [Required(ErrorMessage = "Surname is a required field.")]
        public string? Surname { get; init; }

        [Required(ErrorMessage = "Phonenumber is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "Active is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

