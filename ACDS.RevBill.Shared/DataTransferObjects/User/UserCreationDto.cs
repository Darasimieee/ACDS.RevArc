using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "Username is a required field.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phonenumber is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Agency Id is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "Account Confirmed is a required field.")]
        public bool AccountConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        [Required(ErrorMessage = "Active is a required field.")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }

        public string? TenantName { get; set; }
    }
}
