using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class BillExistDto
    {
        public long BillId { get; set; }
        public string BillReferenceNo { get; set; }
        public string HarmonizedBillReferenceNo { get; set; }
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

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}
