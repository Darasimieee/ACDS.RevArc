using System;
using System.ComponentModel.Design;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class UserRoleRepository : RepositoryBase<UserRoles>, IUserRoleRepository
    {
        public UserRoleRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<UserRoles>> GetAllUserRolesAsync(int OrganisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            var users = await FindByCondition(e => e.OrganisationId.Equals(OrganisationId), trackChanges)
                .OrderBy(e => e.UserRoleId)
                .Include(o => o.Roles)
                .ToListAsync();

            return PagedList<UserRoles>
                .ToPagedList(users, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<UserRoles> GetUserRoleAsync(int OrganisationId, int UserRoleId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(OrganisationId) && c.UserRoleId.Equals(UserRoleId), trackChanges)
                .OrderBy(e => e.UserRoleId)
                .Include(o => o.Roles)
                .SingleOrDefaultAsync();

        public void AddUserToRole(int OrganisationId, int UserId, UserRoles userRoles)
        {
            userRoles.UserId = UserId;
            userRoles.OrganisationId = OrganisationId;
            Create(userRoles);
        }
    }
}

