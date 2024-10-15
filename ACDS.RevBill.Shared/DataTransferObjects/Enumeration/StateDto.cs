using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class StateDto
	{
        public int Id { get; set; }
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
        public int? CountryId { get; set; }
        public string? Capital { get; set; }
    }
}

