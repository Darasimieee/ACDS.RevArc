using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class UpdateOrganisationModuleDto
    {
        [Required(ErrorMessage = "Module Id is a required field.")]
        public int ModuleId { get; init; }

        [Required(ErrorMessage = "Active status is a required field.")]
        public int Status { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifieddBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

