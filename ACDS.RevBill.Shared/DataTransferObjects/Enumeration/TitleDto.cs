using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class TitleDto
	{
        public int TitleId { get; init; }
        public string? TitleCode { get; init; }
        public string? TitleName { get; init; }
    }
}

