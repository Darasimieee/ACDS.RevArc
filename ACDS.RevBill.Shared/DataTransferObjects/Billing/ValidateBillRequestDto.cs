using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class ValidateBillRequestDto
	{
        public string? WebGuid { get; set; }

        public string? State { get; set; }

        public string? Hash { get; set; }

        public string? ClientID { get; set; }

        public string? TellerID { get; set; }

        public string? Currency { get; set; }

        public string? Type { get; set; }

        public string? CBNCode { get; set; }

        public string? Date { get; set; }
    }
}

