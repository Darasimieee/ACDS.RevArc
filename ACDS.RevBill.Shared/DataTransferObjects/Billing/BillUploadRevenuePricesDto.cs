using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices
{
    public class BillUploadRevenuePricesDto
    {
   
        [Required(ErrorMessage = "Revenue Name is a required field.")]
        public string? RevenueCode { get; init; }
        [Required(ErrorMessage = "Category is a required field.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "BillAmount is a required field.")]
        public decimal BillAmount { get; set; }
    }
}

