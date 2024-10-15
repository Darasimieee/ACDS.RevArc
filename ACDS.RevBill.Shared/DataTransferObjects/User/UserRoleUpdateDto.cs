using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserRoleUpdateDto
    {
        [Required(ErrorMessage = "RoleId is a required field.")]
        public int RoleId { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string ModifiedBy { get; init; }
    }
}

