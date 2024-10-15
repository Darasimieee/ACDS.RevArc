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
    internal sealed class EmailAccountRepository : RepositoryBase<EmailAccounts>, IEmailAccountRepository
    {
        public EmailAccountRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<PagedList<EmailAccounts>> GetAllEmailAccounts(RoleParameters requestParameters, bool trackChanges)
        {
            var emailAccounts = await FindAll(trackChanges)
                .OrderBy(e => e.Email)
                .ToListAsync();

            return PagedList<EmailAccounts>
                .ToPagedList(emailAccounts, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<EmailAccounts> GetEmailAccount(int Id, bool trackChanges) =>
            await FindByCondition(c => c.EmailAccountId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmailAccount(EmailAccounts emailAccounts) => Create(emailAccounts);

        // public void DeleteLga(Lgas lgas) => Delete(lgas);

    }
  
}
