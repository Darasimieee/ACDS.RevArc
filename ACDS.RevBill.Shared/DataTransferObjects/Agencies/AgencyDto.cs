using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record AgencyDto(int AgencyId, int OrganisationId, string AgencyCode, string AgencyName);

}