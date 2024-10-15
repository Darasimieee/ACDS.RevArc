using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IRoleModuleMenuService
    {
       Task<(IEnumerable<GetRoleModMenuDto> RoleModules, MetaData metaData)> GetAllRoleModuleMenusAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges);
       Task<GetRoleModMenuDto> GetRoleModuleMenuAsync(int organisationId, int roleModuleMenuId, bool trackChanges);
        Task<Response> CreateRoleModuleMenuAsync(int organisationId, int roleId, List<CreateRoleModMenusDto> RoleModule);
        Task<Response> UpdateRoleModuleMenusAsync(int organisationId, int roleId, List<UpdateRoleModMenusDto> updateRoleModuleMenu, bool trackChanges);
        }
}