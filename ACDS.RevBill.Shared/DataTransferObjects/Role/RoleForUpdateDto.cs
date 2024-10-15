using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class RoleForUpdateDto
    {
        [Required(ErrorMessage = "Role Name is a required field.")]
        public string? RoleName { get; init; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Status { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = " ModifiedBy is a required field.")]
        public string ModifiedBy { get; init; }
    }
}

