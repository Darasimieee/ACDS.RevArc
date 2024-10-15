using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public record GetBillFormat(int BillFormatId, int OrganisationId, byte[] SignatureOneData, string SignatureOneName, byte[] SignatureTwoData, string SignatureTwoName, string ContentOne,
        string ContentTwo, string ClosingSection);
}