using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class RoleModuleMenuRepository : RepositoryBase<RoleModuleMenus>, IRoleModuleMenuRepository
    {
        public RoleModuleMenuRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<RoleModuleMenus>> GetAllRoleModuleMenusAsync(int organisationId, int roleId,  RoleParameters requestParameters, bool trackChanges)
        {
            var organisationRoles = await FindByCondition(e => e.OrganisationId.Equals(organisationId), trackChanges)
                .OrderBy(e => e.RoleModuleMenusId)
                .Include(o => o.Modules)
                .Include(o => o.Roles)
                .Include(o =>o.Menus)
                .ToListAsync();

            return PagedList<RoleModuleMenus>
                .ToPagedList(organisationRoles, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<RoleModuleMenus> GetRoleModuleMenuAsync(int organisationId, int roleModuleMenuId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.RoleModuleMenusId.Equals(roleModuleMenuId), trackChanges)
            .Include(o => o.Modules)
            .Include(o => o.Roles)
            .Include(o => o.Menus)
            .SingleOrDefaultAsync();

        public async Task<RoleModuleMenus> CheckModuleMenuInRoleAsync(int organisationId, int roleId, int moduleId,int menuId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.RoleId.Equals(roleId) && c.ModuleId.Equals(moduleId) &&c.MenuId.Equals(menuId), trackChanges)
            .Include(o => o.Modules)
            .Include(o => o.Roles)
            .Include(o => o.Menus)
            .SingleOrDefaultAsync();

     public void CreateRoleModuleMenu(RoleModuleMenus roleModuleMenus) => Create(roleModuleMenus);

        //public void CreateRoleModule(int organisationId, RoleModuleMenus roleModulemenus)
        //{
        //    roleModulemenus.OrganisationId = organisationId;
        //    Create(roleModulemenus);
        //}
    }   
}