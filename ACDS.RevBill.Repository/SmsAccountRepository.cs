using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Repository
{
    internal sealed class SmsAccountRepository : RepositoryBase<SmsAccounts>, ISmsAccountRepository
    {
        public SmsAccountRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<PagedList<SmsAccounts>> GetAllSmsAccounts(RoleParameters requestParameters, bool trackChanges)
        {
            var smsAccounts = await FindAll(trackChanges)
                .OrderBy(e => e.Username)
                .ToListAsync();

            return PagedList<SmsAccounts>
                .ToPagedList(smsAccounts, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<SmsAccounts> GetSmsAccount(int Id, bool trackChanges) =>
            await FindByCondition(c => c.SmsAccountId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateSmsAccount(SmsAccounts smsAccounts) => Create(smsAccounts);
    }
}
