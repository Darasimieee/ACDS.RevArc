using System;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public class Bill
	{
        public int Id { get; set; }
        public string HarmonisedBillReference { get; set; }
        public List<GetBillDto> Records { get; set; }
        public decimal TotalAmount { get; set; }
    }
}