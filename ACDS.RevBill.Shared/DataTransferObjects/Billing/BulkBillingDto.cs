using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class BulkBillingDto
	{
        public string BuildingNo { get; set; }
        public string? BuildingName { get; set; }
        public string? PayerID { get; set; }
        public string? AgencyCode { get; set; }
        public string? RevenueCode { get; set; }
        public string? Category { get; set; }
        public string? BusinessType { get; set; }
        public decimal BillAmount { get; set; }
        public string? AppliedDate { get; set; }
        public int Year { get; set; }
        public string? CreatedBy { get; set; }
    }
}