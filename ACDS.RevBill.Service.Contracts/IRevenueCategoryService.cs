using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IRevenueCategoryService
    {
        Task<(IEnumerable<RevenueCategoryDto> revenueCategories, MetaData metaData)> GetAllRevenueCategoriesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<RevenueCategoryDto> revenueCategories, MetaData metaData)> GetOrganisationsRevenueCategoryAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<RevenueCategoryDto> GetRevenueCategoryAsync(int Id, bool trackChanges);
       
        Task<RevenueCategoryDto> CreateRevenueCategoryAsync(int revenueId, RevenueCategoryCreationDto revenueCategory);
        Task UpdateRevenueCategoryAsync(int Id, RevenueCategoryUpdateDto revenueCategoryUpdate, bool trackChanges);
    }
}

