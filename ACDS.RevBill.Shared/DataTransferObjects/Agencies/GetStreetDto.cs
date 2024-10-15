using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetStreetDto(int StreetId, int AgencyId, int OrganisationId,string StreetName);

}