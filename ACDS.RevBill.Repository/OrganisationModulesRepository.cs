using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class OrganisationModulesRepository : RepositoryBase<OrganisationModules>, IOrganisationModulesRepository
    {
        public OrganisationModulesRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<OrganisationModules>> GetAllOrganisationModulesAsync(int organisationId, RoleParameters requestParameters, bool trackChanges)
        {
            var organisations = await FindAll(trackChanges)
                .OrderBy(e => e.OrganisationId == organisationId && e.Status == ((int)Status.Active))
                .Include(o => o.Modules)
                .ToListAsync();

            return PagedList<OrganisationModules>
                .ToPagedList(organisations.Where(x => x.OrganisationId== organisationId && x.Status== ((int)Status.Active)), requestParameters.PageNumber, requestParameters.PageSize);
        }

        //get org module 
        public async Task<List<OrganisationModules>> GetOrgModuleAsync(int organisationId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) , trackChanges)
            .Include(o => o.Modules)
            .ToListAsync();   
   
        //get only active organisationmodules
        public async Task<OrganisationModules> GetOrganisationModuleAsync(int moduleId,int organisationId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.ModuleId == moduleId && c.Status == ((int)Status.Active), trackChanges)
            .Include(o => o.Modules)
          .SingleOrDefaultAsync();

        public async Task<OrganisationModules> GetOrganisationIdModuleAsync(int organisationModuleId, bool trackChanges) =>
          await FindByCondition(c => c.OrganisationModuleId.Equals(organisationModuleId), trackChanges)
            .Include(o => o.Modules)
            .SingleOrDefaultAsync();

        public void CreateOrganisationModule(OrganisationModules organisationModules) => Create(organisationModules);       
    }   
}

