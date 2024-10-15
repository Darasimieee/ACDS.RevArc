using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class PayerIdEnumerationDto
	{
        [Required(ErrorMessage = "PayerId is a required field.")]
        [RegularExpression(@"^(N|C)-.*$", ErrorMessage = "The PayerID must start with 'N-' or 'C-'")] 
        public string? PayerId { get; set; }
    }
}

