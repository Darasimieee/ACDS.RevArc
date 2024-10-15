using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class PaymentByAgency
    {
        public int Id { get; set; }
        public string? AgencyName { get; set; }
        public int BillCount { get; set; }
        public string? BillValue { get; set; }
        public string? BillPaid { get; set; }
        public string? BillOutstanding { get; set; }
    }
}