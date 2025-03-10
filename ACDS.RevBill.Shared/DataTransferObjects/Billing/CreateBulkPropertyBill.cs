using System;
using System.ComponentModel;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class CreateBulkPropertyBill
    {
        public List<CreatePropertyBill> CreatePropertyBillDto { get; set; }
        [DefaultValue(false)]
        public bool DeactivateArears { get; set; }
        public string? Approver { get; set; }
    }
}