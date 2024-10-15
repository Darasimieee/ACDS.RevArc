using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class AgencyRequestDto
	{
        public string? State { get; set; }
        public string? ClientId { get; set; }
        public string? Hash { get; set; }
    }
}

