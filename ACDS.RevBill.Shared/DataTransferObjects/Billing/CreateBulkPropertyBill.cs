using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreateBulkPropertyBill
	{
		public List<CreatePropertyBill> CreatePropertyBillDto { get; set; }
	}
}