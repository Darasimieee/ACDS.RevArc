using System;
using System.ComponentModel.DataAnnotations;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreatePropertyBill
	{
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "RevenueId is a required field.")]
        public List<BillRevenuePricesDto>? BillRevenuePrices { get; set; }        
        public int BusinessTypeId { get; set; }
        [Required(ErrorMessage = "Business Size is a required field.")]
        public int BusinessSizeId { get; set; }
   
        [Required(ErrorMessage = "AppliedDate is a required field.")]
        public string? AppliedDate { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

