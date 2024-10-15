using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreateAutoBillDto
	{
        [Required(ErrorMessage = "BillId is a required field.")]
        public long BillId { get; set; }

        [Required(ErrorMessage = "AppliedDate is a required field.")]
        public string? AppliedDate { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}