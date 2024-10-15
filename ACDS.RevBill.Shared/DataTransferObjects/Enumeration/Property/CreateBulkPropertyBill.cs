using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreateBulkProperty
	{
		public List<CreatePropertyDto> CreateProperty { get; set; }
	}
}