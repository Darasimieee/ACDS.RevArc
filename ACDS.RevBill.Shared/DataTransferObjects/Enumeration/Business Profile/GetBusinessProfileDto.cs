using System;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile
{
    public record GetBusinessProfileDto(int BusinessProfileId, int OrganisationId, int PropertyId, GetBusinessTypeDto BusinessType, GetBusinessSizeDto BusinessSize,
        RevenueDto Revenues);
}

