using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class AgencySummaryDto
	{
        public int Id { get; set; }
        public string? AgencyName { get; set; }
        public int PropertyCount { get; set; }
        public int CustomerCount { get; set; }
    }
}