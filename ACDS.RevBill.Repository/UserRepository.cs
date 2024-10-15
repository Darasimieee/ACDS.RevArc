using System;
using System.ComponentModel.Design;
using System.Runtime.Intrinsics.X86;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class UserRepository : RepositoryBase<Users>, IUsersRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Users>> GetAllUsersAsync(int OrganisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            var users = await FindByCondition(e => e.OrganisationId.Equals(OrganisationId), trackChanges)
           .OrderBy(e => e.UserId)
           .ToListAsync();

            return PagedList<Users>
                .ToPagedList(users, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Users> GetUserAsync(int OrganisationId, int UserId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(OrganisationId) && c.UserId.Equals(UserId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<Users> GetEmailAsync(int OrganisationId, string Email, bool trackChanges) =>
           await FindByCondition(c => c.OrganisationId.Equals(OrganisationId) && c.Email.Equals(Email), trackChanges)
           .SingleOrDefaultAsync();

        public async Task<Users> GetUsernameAsync(int OrganisationId, string Username, bool trackChanges) =>
          await FindByCondition(c => c.OrganisationId.Equals(OrganisationId) && c.UserName.Equals(Username), trackChanges)
        .SingleOrDefaultAsync();

        public void CreateUserForOrganisation(int OrganisationId, Users users)
        {
            users.OrganisationId = OrganisationId;
            Create(users);
        }
    }
}

