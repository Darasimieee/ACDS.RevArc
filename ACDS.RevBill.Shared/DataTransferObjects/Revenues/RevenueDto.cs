using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record RevenueDto(int RevenueId, int OrganisationId, int BusinessTypeId,  string RevenueCode, string RevenueName, string Description);

}

