using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record CategoryDto(int CategoryId, int OrganisationId, int BusinessSizeId, string CategoryName, int PayerTypeId, string Description, bool Active);

}

