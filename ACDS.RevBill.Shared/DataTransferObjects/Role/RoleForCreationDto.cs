using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class RoleForCreationDto
    {
        [Required(ErrorMessage = "Role Name is a required field.")]
        public string? RoleName { get; init; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Status { get; init; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string CreatedBy { get; init; }
    }
}

