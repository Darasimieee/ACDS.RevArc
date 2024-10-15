using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class RoleModulesRepository : RepositoryBase<RoleModules>, IRoleModulesRepository
    {
        public RoleModulesRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<RoleModules>> GetAllRoleModulesAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges)
        {
            var organisationRoles = await FindByCondition(e => e.OrganisationId.Equals(organisationId), trackChanges)
                .OrderBy(e => e.RoleModuleId)
                .Include(o => o.Modules)
                .Include(o => o.Roles)
                .ToListAsync();

            return PagedList<RoleModules>
                .ToPagedList(organisationRoles, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<RoleModules> GetRoleModuleAsync(int organisationId, int roleId,int moduleId , bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.ModuleId.Equals(moduleId) && c.RoleId.Equals(roleId), trackChanges)
            .Include(o => o.Modules)
            .Include(o => o.Roles)
            .SingleOrDefaultAsync();

        public async Task<RoleModules> CheckModuleInRoleAsync(int organisationId, int roleId, int moduleId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.RoleId.Equals(roleId) && c.ModuleId.Equals(moduleId), trackChanges)
            .Include(o => o.Modules)
            .Include(o => o.Roles)
            .SingleOrDefaultAsync();

        //public void CreateRoleModule(RoleModules RoleModules) => Create(RoleModules);

        public void CreateRoleModule(int organisationId, RoleModules roleModules)
        {
            roleModules.OrganisationId = organisationId;
            Create(roleModules);
        }
    }   
}