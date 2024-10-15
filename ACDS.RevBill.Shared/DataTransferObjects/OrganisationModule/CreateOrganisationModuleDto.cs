using System;
using System.ComponentModel.DataAnnotations;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class CreateOrganisationModuleDto
    {       
        [Required(ErrorMessage = "Module Id is a required field.")]
        public int ModuleId { get; init; }

        [Required(ErrorMessage = "Active status is a required field.")]
        public int Status { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }
    }
}
