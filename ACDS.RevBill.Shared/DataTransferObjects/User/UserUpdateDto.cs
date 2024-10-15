using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Username is a required field.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Email is a required field.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Phonenumber is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "Account Confirmed is a required field.")]
        public bool AccountConfirmed { get; init; }

        public bool LockoutEnabled { get; init; }

        [Required(ErrorMessage = "Active is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "Date Modified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string ModifiedBy { get; init; }
    }
}

