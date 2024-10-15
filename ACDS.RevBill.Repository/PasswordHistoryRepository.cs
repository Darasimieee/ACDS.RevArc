using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Repository
{
    internal sealed class PasswordHistoryRepository : RepositoryBase<PasswordHistory>, IPasswordHistoryRepository
    {
        public PasswordHistoryRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public void AddPassword(int UserId, PasswordHistory passwordHistory)
        {
            passwordHistory.UserId = UserId;
            Create(passwordHistory);
        }
    }
}

