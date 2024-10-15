using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IUserRoleRepository
	{
        Task<PagedList<UserRoles>> GetAllUserRolesAsync(int OrganisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<UserRoles> GetUserRoleAsync(int OrganisationId, int UserRoleId, bool trackChanges);
        void AddUserToRole(int OrganisationId, int UserId, UserRoles userRoles);
    }
}

