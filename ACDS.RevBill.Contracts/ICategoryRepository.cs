using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface ICategoryRepository
    {
        Task<PagedList<Category>> GetAllCategoriesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Category>> GetCategoriesbyOrgAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<IEnumerable<Category>> GetCategorybyBusinessSizeAsync(int businessSizeId,int organisationId, bool trackChanges);
        Task<Category> GetCategorybyOrganisationId(int organisationId, string categoryName,int businessSizeId, bool trackChanges);
        Task<Category> GetCategoryAsync(int Id, bool trackChanges);
        void CreateCategoryAsync(Category category);
    }
}

