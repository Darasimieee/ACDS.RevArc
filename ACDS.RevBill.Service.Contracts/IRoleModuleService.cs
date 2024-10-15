using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IRoleModuleservice
	{
       Task<(IEnumerable<GetRoleModuleDto> RoleModules, MetaData metaData)> GetAllRoleModulesAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges);
       Task<GetRoleModuleDto> GetRoleModuleAsync(int organisationId, int roleId, int moduleId, bool trackChanges);
       Task<Response> CreateRoleModuleAsync(int organisationId, int roleId, List<CreateRoleModuleDto> orgRoleModule);
       Task<Response> UpdateRoleModuleAsync(int organisationId, int roleId, int moduleId, UpdateRoleModuleDto updateRoleModuleOrga, bool trackChanges);
    }
}