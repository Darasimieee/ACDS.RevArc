using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class UpdateBillRequest
    {
        public decimal Amount { get; set; }
        public string? Editedby { get; set; }
        public string? BillReference { get; set; }
    }
}