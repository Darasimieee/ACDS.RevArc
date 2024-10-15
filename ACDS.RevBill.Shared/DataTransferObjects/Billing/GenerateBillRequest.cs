using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class GenerateBillRequest
	{
        public string? PayerID { get; set; }
        public string? AgencyRef { get; set; }
        public string? RevCode { get; set; }
        public decimal Amount { get; set; }
        public string? EntryDate { get; set; }
        public string? AssessRef { get; set; }
        public string? AppliedDate { get; set; }
        public string? Year { get; set; }
        public string? BillReference { get; set; }
        public string? HarmonizedBillReference { get; set; }
        public string? PropertyAddress { get; set; }
    }
}