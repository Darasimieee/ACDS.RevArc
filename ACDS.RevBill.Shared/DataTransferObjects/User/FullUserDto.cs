using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class FullUserDto
    {
        public int UserId { get; set; }
        public int UserProfileId { get; set; }
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public int UserPasswordId { get; set; }
        public int OrganisationId { get; set; }

        [Required(ErrorMessage = "Username is a required field.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Firstname is a required field.")]
        public string Firstname { get; set; }
        public string Middlename { get; set; }

        [Required(ErrorMessage = "Surname is a required field.")]
        public string Surname { get; set; }

        public string ZipCode { get; set; }
        public int LgaId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool AccountConfirmed { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string TenantName { get; set; }

        public FullUserDto () { }
    }
}

