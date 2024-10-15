using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class UpdateBilldto
    {
        [Required(ErrorMessage = "BillId is a required field.")]
        public long BillId { get; set; }
        
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "RevenueId is a required field.")]
        public int RevenueId { get; set; }

        [Required(ErrorMessage = "BillAmount is a required field.")]
        public decimal BillAmount { get; set; }

        [Required(ErrorMessage = "Category is a required field.")]
        public string? Category { get; set; }
        [Required(ErrorMessage = "Business Type is a required field.")]
        public int? BusinessTypeId { get; set; }
        [Required(ErrorMessage = "Business Size is a required field.")]
        public int? BusinessSizeId { get; set; }
        [Required(ErrorMessage = "AppliedDate is a required field.")]
        public string? AppliedDate { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}