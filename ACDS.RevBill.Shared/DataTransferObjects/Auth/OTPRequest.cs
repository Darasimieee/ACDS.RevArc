using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Auth
{
    public class OTPRequest
    {
        [Required]
        public string? Email { get; set; }
    }
}

