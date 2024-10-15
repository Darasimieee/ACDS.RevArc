using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class AgencyRepository : RepositoryBase<Agencies>, IAgencyRepository
    {
        public AgencyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Agencies>> GetAllAgenciesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var agencies = await FindAll(trackChanges)
                .OrderBy(e => e.AgencyName)
                .ToListAsync();

            return PagedList<Agencies>
                .ToPagedList(agencies, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<PagedList<Agencies>> GetAllAgenciesbyOrgAsync(int Id,RoleParameters roleParameters, bool trackChanges)
        {
            var agencies = await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
                .OrderBy(e => e.AgencyName)
                .ToListAsync();

            return PagedList<Agencies>
                .ToPagedList(agencies, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<Agencies> GetAgencyAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.AgencyId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<Agencies> GetAgencybyIdOrgId(int Id,int organisationId, bool trackChanges) =>
    await FindByCondition(c => c.AgencyId.Equals(Id) && c.OrganisationId.Equals(organisationId), trackChanges)
    .SingleOrDefaultAsync();
   
        public async Task<Agencies> GetAgencybynameAsync(int organisationId,string  name, bool trackChanges) =>
           await FindByCondition(c => c.OrganisationId == organisationId && c.AgencyName.Equals(name), trackChanges)
           .SingleOrDefaultAsync();
        public void CreateAgencyAsync(Agencies agencies) => Create(agencies);

     
    }   
}

