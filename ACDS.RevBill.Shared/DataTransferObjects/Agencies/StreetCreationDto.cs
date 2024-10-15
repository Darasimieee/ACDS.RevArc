using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class StreetCreationDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "Agency Id is a required field.")]
        public int AgencyId { get; init; }
        [Required(ErrorMessage = "Street is a required field.")]
        public string? StreetName { get; init; } 

        [Required(ErrorMessage = "Description is a required field.")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }
    }
}

