using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
namespace ACDS.RevBill.Contracts
{
	public interface IRoleModulesRepository
    {
        Task<PagedList<RoleModules>> GetAllRoleModulesAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges);
        Task<RoleModules> GetRoleModuleAsync(int organisationId, int roleId,int moduleId, bool trackChanges);
        Task<RoleModules> CheckModuleInRoleAsync(int organisationId, int roleId, int moduleId, bool trackChanges);
        void CreateRoleModule(int organisationId, RoleModules roleModules);       
    }
}