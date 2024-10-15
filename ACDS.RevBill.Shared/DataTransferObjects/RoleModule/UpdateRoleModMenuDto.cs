using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class UpdateRoleModMenuDto
    {
        [Required(ErrorMessage = "Module Id is a required field.")]
        public int ModuleId { get; init; }       

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}