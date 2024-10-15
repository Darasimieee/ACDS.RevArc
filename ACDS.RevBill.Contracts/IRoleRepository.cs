using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IRoleRepository
	{
        Task<PagedList<Roles>> GetAllRolesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<Roles> GetRoleAsync(int Id, bool trackChanges);
		void CreateRole(Roles roles);
        void DeleteRole(Roles roles);
    }
}

