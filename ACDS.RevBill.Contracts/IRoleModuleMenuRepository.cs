using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
namespace ACDS.RevBill.Contracts
{
	public interface IRoleModuleMenuRepository
    {
        Task<PagedList<RoleModuleMenus>> GetAllRoleModuleMenusAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges);
        Task<RoleModuleMenus> GetRoleModuleMenuAsync(int organisationId, int roleModuleMenuId, bool trackChanges);
        Task<RoleModuleMenus> CheckModuleMenuInRoleAsync(int organisationId, int roleId, int moduleId,int menuId, bool trackChanges);
        void CreateRoleModuleMenu(RoleModuleMenus roleModuleMenus);       
    }
}