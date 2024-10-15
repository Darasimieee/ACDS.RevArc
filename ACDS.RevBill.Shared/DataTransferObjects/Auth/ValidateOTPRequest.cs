using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Auth
{
    public class ValidateOTPRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? OTP { get; set; }
    }
}

