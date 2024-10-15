using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
	public record GetPaymentGatewayDto(int BankId, string BankName, string BankUrl, string BankDescription, string BankLogoName, byte[] BankLogoData, bool BankStatus);
}
