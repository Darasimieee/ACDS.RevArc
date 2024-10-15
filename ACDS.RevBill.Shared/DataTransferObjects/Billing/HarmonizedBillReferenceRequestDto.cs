using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class HarmonizedBillReferenceRequestDto
	{
        [Required(ErrorMessage = "HarmonizedBillReference is a required field.")]
        public string? HarmonizedBillReference { get; set; }
    }
}