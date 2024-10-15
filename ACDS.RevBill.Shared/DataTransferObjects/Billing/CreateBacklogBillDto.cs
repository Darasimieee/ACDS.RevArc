using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreateBacklogBillDto
	{
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "RevenueId is a required field.")]
        public int RevenueId { get; set; }

        [Required(ErrorMessage = "BillAmount is a required field.")]
        public decimal BillAmount { get; set; }

        [Required(ErrorMessage = "Year is a required field.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "AppliedDate is a required field.")]
        public string? AppliedDate { get; set; }

        [Required(ErrorMessage = "Category is a required field.")]
        public string? Category { get; set; }
        [Required(ErrorMessage = "BusinessType is a required field.")]
        public string? BusinessType { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

