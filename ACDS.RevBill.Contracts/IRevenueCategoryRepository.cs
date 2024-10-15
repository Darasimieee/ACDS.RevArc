using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IRevenueCategoryRepository
	{
        Task<PagedList<RevenueCategories>> GetAllRevenueCategoriesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<RevenueCategories>> GetRevenueCategorybyOrgAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<RevenueCategories> RevenueCategoryExists(int organisationId, int revenueId, string revenueName, bool trackChanges);
        Task<RevenueCategories> GetRevenueCategoryAsync(int Id, bool trackChanges);
        void CreateRevenueCategoryAsync(RevenueCategories revenueCategories);
    }
}

