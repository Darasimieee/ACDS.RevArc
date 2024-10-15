using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
    public record GetPropertiesDto(int PropertyId, int OrganisationId, AgencyDto Agencies,GetSpaceIdentifierDto SpaceIdentifier, string BuildingNo,
        string LocationAddress, int SpaceFloor, string BuildingName);
}