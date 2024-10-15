using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class AgencyCreationDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "Agency Code is a required field.")]
        public string? AgencyCode { get; init; }
        [Required(ErrorMessage = "Agency Name is a required field.")]
        public string? AgencyName { get; init; }
        [Required(ErrorMessage = "Description is a required field.")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }
        public int? DepartmentId { get; init; }
    }
}

