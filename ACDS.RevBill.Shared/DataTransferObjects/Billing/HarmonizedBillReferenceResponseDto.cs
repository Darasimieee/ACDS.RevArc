using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class HarmonizedBillReferenceResponseDto
	{
		public string? EntryID { get; set; }
        public string? PayerType { get; set; }
        public string? PayerID { get; set; }
        public string? AgencyRef { get; set; }
        public string? RevCode { get; set; }
        public string? Amount { get; set; }
        public string? EntryDate { get; set; }
        public string? WebGUID { get; set; }
        public string? AssessRef { get; set; }
        public string? AppliedDate { get; set; }
        public string? Notes { get; set; }
        public string? Sysdate { get; set; }
        public string? fullname { get; set; }
        public string? Address { get; set; }
        public string? email { get; set; }
        public string? gsm { get; set; }
        public string? BillOwner { get; set; }
        public string? PmtStus { get; set; }
        public string? BulkReference { get; set; }
        public string? BulkTotalAmount { get; set; }
        public string? Pmt_Flag { get; set; }
    }
}