using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserRoleCreationDto
    {
        [Required(ErrorMessage = "RoleId is a required field.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }

        public string? TenantName { get; set; }
    }
}

