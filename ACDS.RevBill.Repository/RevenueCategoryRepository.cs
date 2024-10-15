using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class RevenueCategoryRepository : RepositoryBase<RevenueCategories>, IRevenueCategoryRepository
    {
        public RevenueCategoryRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<RevenueCategories>> GetAllRevenueCategoriesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var revenueCategories = await FindAll(trackChanges)
                .OrderBy(e => e.CategoryName)
                .ToListAsync();

            return PagedList<RevenueCategories>
                .ToPagedList(revenueCategories, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<PagedList<RevenueCategories>> GetRevenueCategorybyOrgAsync(int Id,RoleParameters roleParameters, bool trackChanges)
        {
            var revenueCategories = await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
                .OrderBy(e => e.CategoryName)
                .ToListAsync();

            return PagedList<RevenueCategories>
                .ToPagedList(revenueCategories, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<RevenueCategories> GetRevenueCategoryAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.RevenueCategoryId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<RevenueCategories> RevenueCategoryExists(int organisationId,int revenueId,string categoryName, bool trackChanges) =>
       await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.CategoryName == categoryName && c.RevenueId== revenueId, trackChanges)
       .SingleOrDefaultAsync();
        
        public void CreateRevenueCategoryAsync(RevenueCategories revenueCategories) => Create(revenueCategories);

     
    }   
}

