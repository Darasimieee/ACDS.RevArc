using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class RevenueUpdateDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "Agency Id is a required field.")]
        public int AgencyId { get; set; }
        [Required(ErrorMessage = "BusinessType Id is a required field.")]
        public int BusinessTypeId { get; set; }
        [Required(ErrorMessage = "Revenue Code is a required field.")]
        public string? RevenueCode { get; init; }
        [Required(ErrorMessage = "Revenue Name is a required field.")]
        public string? RevenueName { get; init; }
        [Required(ErrorMessage = "Description is a required field.")] 
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = " ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

