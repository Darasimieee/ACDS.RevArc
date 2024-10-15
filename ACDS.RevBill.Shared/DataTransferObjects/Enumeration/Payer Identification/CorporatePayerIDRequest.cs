using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class CorporatePayerIDRequest
	{
        [Required(ErrorMessage = "CompnayName is a required field.")]
        public string? CompanyName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is a required field.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "DateofIncorporation is a required field.")]
        public string? DateofIncorporation { get; set; }
    }
}