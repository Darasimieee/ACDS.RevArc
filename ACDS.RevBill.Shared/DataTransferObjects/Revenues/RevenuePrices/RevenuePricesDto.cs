using System;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices
{
    public record RevenuePricesDto(int RevenuePriceId, int OrganisationId, int CategoryId, string CategoryName, int RevenueId,decimal Amount, bool Active,
        DateTime DateCreated, string CreatedBy, DateTime DateModified, string ModifiedBy);

}

