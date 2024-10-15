using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
    public record GetOrganisationPaymentGatewayDto(int OrganisationBankId, int OrganisationId, GetPaymentGatewayDto Banks, bool BankStatus);

}