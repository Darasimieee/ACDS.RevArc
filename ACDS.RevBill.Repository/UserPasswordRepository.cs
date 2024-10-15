using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class UserPasswordRepository : RepositoryBase<UserPasswords>, IUserPasswordRepository
    {
        public UserPasswordRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<UserPasswords> GetUserPasswordAsync(int UserPasswordId, bool trackChanges) =>
            await FindByCondition(c => c.UserPasswordId.Equals(UserPasswordId), trackChanges)
            .SingleOrDefaultAsync();

        public void AddUserPassword(int OrganisationId, int UserId, UserPasswords userPasswords)
        {
            userPasswords.UserId = UserId;
            userPasswords.OrganisationId = OrganisationId;
            Create(userPasswords);
        }
    }
}

