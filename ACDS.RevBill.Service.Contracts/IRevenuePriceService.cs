using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IRevenuePriceService
    {
        Task<(IEnumerable<RevenuePricesDto> revenuePrices, MetaData metaData)> GetAllRevenuePricesAsync(RoleParameters roleParameters, bool trackChanges);
       // Task<(IEnumerable<RevenuePriceListDto> revenuePrices, MetaData metaData)> GetOrganisationsRevenuePricesAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<RevenuePriceListDto> revenuePriceList, MetaData metaData)> GetOrganisationsRevenuePricesAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<RevenuePricesDto> GetRevenuePriceAsync(int Id, bool trackChanges);
        Task<(IEnumerable<RevenuePricesDto> revenuePrices, MetaData metaData)> GetCategorysRevenuePricesAsync(int organisationId, int categoryId, RoleParameters roleParameters, bool trackChanges);
        Task<RevenuePricesDto> CreateRevenuePriceAsync(int revenueId,RevenuePricesCreationDto revenuePrice);
        
        Task<IEnumerable<RevenuePricesDto>> GetRevenuePriceByRevenueAsync(int RevenueId, bool trackChanges);
        Task UpdateRevenuePriceAsync(int Id, RevenuePricesUpdateDto revenuePriceUpdate, bool trackChanges);
    }
}

