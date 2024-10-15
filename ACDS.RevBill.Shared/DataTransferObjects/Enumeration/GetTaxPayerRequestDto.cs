using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class GetTaxPayerRequestDto
	{
        [Required(ErrorMessage = "This is a required field. Enter a name, phone number or email address.")]
        public string? Param { get; set; }
    }
}

