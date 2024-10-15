using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserProfileCreationDto
    {
        public int UserRoleId { get; set; }

        [Required(ErrorMessage = "Firstname is a required field.")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Surname is a required field.")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phonenumber is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string? PhoneNumber { get; set; }


        [Required(ErrorMessage = "Active is a required field.")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }

        public string? TenantName { get; set; }
    }
}

