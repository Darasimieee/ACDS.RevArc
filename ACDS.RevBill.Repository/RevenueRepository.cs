using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class RevenueRepository : RepositoryBase<Revenues>, IRevenueRepository
    {
        public RevenueRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Revenues>> GetAllRevenuesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var revenues = await FindAll(trackChanges)
                .OrderBy(e => e.RevenueName).Distinct()
                .ToListAsync();

            return PagedList<Revenues>
                .ToPagedList(revenues, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<Revenues> GetRevenueByIdAsync(int revenueId, bool trackChanges)
        {
            // Use FindByCondition to leverage the pattern used in GetAllRevenuesAsync
            var revenue = await FindByCondition(r => r.RevenueId == revenueId, trackChanges)
                .FirstOrDefaultAsync();

            if (revenue == null)
            {
                throw new KeyNotFoundException("Revenue not found.");
            }

            return revenue;
        }

        // Other methods...
    
    public async Task<PagedList<Revenues>> GetRevenuesbyOrgAsync(int Id,RoleParameters roleParameters, bool trackChanges)
        {
            var revenues = await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
               
                .OrderBy(e => e.RevenueName)
                .ToListAsync();

           

            return PagedList<Revenues>
                .ToPagedList(revenues, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<PagedList<Revenues>> GetRevenuesbyBusinessTypeAsync(int organisationId, int businesstypeId, RoleParameters roleParameters, bool trackChanges)
        {
            var revenues = await FindByCondition(c => c.OrganisationId.Equals(organisationId)  && c.BusinessTypeId== businesstypeId, trackChanges)
                .OrderBy(e => e.RevenueName)
                .ToListAsync();

            return PagedList<Revenues>
                .ToPagedList(revenues, roleParameters.PageNumber, roleParameters.PageSize);
        }        
        public async Task<Revenues> GetRevenueAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.RevenueId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<Revenues> GetRevenuebyOrganisationId(int organisationId,string revenueName, bool trackChanges) =>
       await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.RevenueName == revenueName, trackChanges)
       .SingleOrDefaultAsync();
        
        public void CreateRevenueAsync(Revenues revenues) => Create(revenues);

        public async Task<Revenues> GetRevenuebyOrganisationIdAndDescription(int organisationId, string revenueName, int businessTypeId, bool trackChanges)
        {
            return await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.RevenueName == revenueName && c.BusinessTypeId==businessTypeId, trackChanges)
       .FirstOrDefaultAsync();
        }
    }   
}

