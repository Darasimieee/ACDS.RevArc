using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class CountriesDto
	{
        public int Id { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? CapitalCity { get; set; }
    }
}

