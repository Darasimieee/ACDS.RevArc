using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IRevenuePriceRepository
    {
        Task<PagedList<RevenuePrices>> GetAllRevenuePricesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<IEnumerable<RevenuePrices>> GetRevenuePricesbyOrgAsync(int organisationId, bool trackChanges);
        Task<PagedList<RevenuePrices>> GetRevenuePricesbyCatAsync(int organisationId,int categoryId, RoleParameters roleParameters, bool trackChanges);
        Task<RevenuePrices> RevenuePriceExists(int organisationId, string categoryName, int revenueId, decimal amount, bool trackChanges);
        Task<RevenuePrices> GetRevenuePriceAsync(int Id, bool trackChanges);
        Task<IEnumerable<RevenuePrices>> GetRevenuePriceByRevenueAsync(int revenueId, bool trackChanges);
        
        void CreateRevenuePriceAsync(RevenuePrices revenue);
    }
}

