using System;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories
{
    public record RevenueCategoryDto(int RevenueCategoryId, int OrganisationId, int CategoryId, string CategoryName, int RevenueId, string Description, bool Active);

}

