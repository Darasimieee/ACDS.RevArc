using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class AgencyBillingSummaryDto
	{
        public int Id { get; set; }
        public string? AgencyName { get; set; }
        public string? PropertyCount { get; set; }
        public string? PropertyAmount { get; set; }
        public string? NonPropertyCount { get; set; }
        public string? NonPropertyAmount { get; set; }
    }
}