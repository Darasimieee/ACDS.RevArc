using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices
{
    public class RevenuePricesCreationDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "Category Name is a required field.")]
        public string? CategoryName { get; init; }

        [Required(ErrorMessage = "Category Id is a required field.")]
        public int CategoryId { get; init; }
        //[Required(ErrorMessage = "Agency Id is a required field.")]
        //public int AgencyId { get; set; }
        [Required(ErrorMessage = "Amount is a required field.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }
    }
}

