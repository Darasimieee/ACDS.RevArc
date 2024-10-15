using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreateNonPropertyBillDto
	{
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "RevenueId is a required field.")]
        public int RevenueId { get; set; }

        [Required(ErrorMessage = "FrequencyId is a required field.")]
        public int FrequencyId { get; set; }

        [Required(ErrorMessage = "BillAmount is a required field.")]
        public decimal BillAmount { get; set; }

        [Required(ErrorMessage = "AppliedDate is a required field.")]
        public string? AppliedDate { get; set; }

        [Required(ErrorMessage = "Category is a required field.")]
        public string? Category { get; set; }
        [Required(ErrorMessage = "Business Type is a required field.")]
        public string? BusinessTypeId { get; set; }
        [Required(ErrorMessage = "Business Size is a required field.")]
        public string? BusinessSizeId { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }

        public List<BillRevenuePrices> BillRevenuePrices { get; set; }


    }

    public class BillRevenuePrices
    {
        public string Category { get; set; }
        public int RevenueId { get; set; }
        public int BillAmount { get; set; }
    }


}