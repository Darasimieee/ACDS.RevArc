using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices
{
    public class BillRevenuePricesDto
    {
   
        [Required(ErrorMessage = "Revenue Name is a required field.")]
        public int RevenueId { get; init; }
        [Required(ErrorMessage = "Category is a required field.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "BillAmount is a required field.")]
        public decimal BillAmount { get; set; }
    }
}

