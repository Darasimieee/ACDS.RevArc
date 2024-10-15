using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices
{
    public class RevenuePricesUpdateDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "Category Name is a required field.")]
        public string? CategoryName { get; init; }

        [Required(ErrorMessage = "Category Id is a required field.")]
        public int CategoryId { get; init; }
        [Required(ErrorMessage = "Revenue Id is a required field.")]
        public int RevenueId { get; set; }
        public string? Description { get; set; }
        //[Required(ErrorMessage = "Agency Id is a required field.")]
        //public int AgencyId { get; set; }
        [Required(ErrorMessage = "Amount is a required field.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = " ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

