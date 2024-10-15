using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IRoleService
	{
        Task<(IEnumerable<RoleDto> roles, MetaData metaData)> GetAllRolesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<RoleDto> GetRoleAsync(int Id, bool trackChanges);
        Task<RoleDto> CreateRoleAsync(RoleForCreationDto role);
        Task DeleteRoleAsync(int Id, bool trackChanges);
        Task UpdateRoleAsync(int Id, RoleForUpdateDto roleForUpdate, bool trackChanges);
    }
}