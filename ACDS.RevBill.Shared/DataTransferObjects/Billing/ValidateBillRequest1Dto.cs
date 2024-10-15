using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class ValidateBillRequest1Dto
	{
        [Required(ErrorMessage = "WebGuid is a required field.")]
        public string? WebGuid { get; set; }
    }
}

