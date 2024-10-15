using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class RevenuePriceRepository : RepositoryBase<RevenuePrices>, IRevenuePriceRepository
    {

        public RevenuePriceRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
          
        }

        public async Task<PagedList<RevenuePrices>> GetAllRevenuePricesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var RevenuePrices = await FindAll(trackChanges)
                .OrderBy(e => e.RevenuePriceId)
                .ToListAsync();
   
            return PagedList<RevenuePrices>
                .ToPagedList(RevenuePrices, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<IEnumerable<RevenuePrices>> GetRevenuePricesbyOrgAsync(int Id, bool trackChanges)
        {
            var RevenuePrices = await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
                .OrderBy(e => e.RevenuePriceId)
                .ToListAsync();

            return RevenuePrices;
        }
        public async Task<PagedList<RevenuePrices>> GetRevenuePricesbyCatAsync(int Id,int categoryId, RoleParameters roleParameters, bool trackChanges)
        {
            var RevenuePrices = await FindByCondition(c => c.OrganisationId.Equals(Id) && c.CategoryId.Equals(categoryId), trackChanges)
                .OrderBy(e => e.RevenuePriceId)
                .ToListAsync();

            return PagedList<RevenuePrices>
                .ToPagedList(RevenuePrices, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<RevenuePrices> GetRevenuePriceAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.RevenuePriceId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<IEnumerable<RevenuePrices>> GetRevenuePriceByRevenueAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.RevenueId.Equals(Id), trackChanges)
            .ToListAsync();
        public async Task<RevenuePrices> RevenuePriceExists(int organisationId, string categoryName, int revenueId,  decimal amount, bool trackChanges) =>
       await FindByCondition(c => c.OrganisationId.Equals(organisationId)&&c.CategoryName== categoryName && c.RevenueId == revenueId  && c.Amount==amount, trackChanges)
       .SingleOrDefaultAsync();
        
        public void CreateRevenuePriceAsync(RevenuePrices RevenuePrices) => Create(RevenuePrices);
       


    }
}

