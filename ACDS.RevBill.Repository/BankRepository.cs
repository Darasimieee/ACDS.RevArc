using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class BankRepository : RepositoryBase<Banks>, IBankRepository
    {
        public BankRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Banks>> GetAllBanksAsync(DefaultParameters requestParameters, bool trackChanges)
        {
            var banks = await FindAll(trackChanges)
                .OrderBy(e => e.BankId)
                .ToListAsync();

            return PagedList<Banks>
                .ToPagedList(banks, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Banks> GetBankAsync(int bankId, bool trackChanges) =>
           await FindByCondition(c => c.BankId.Equals(bankId), trackChanges)
           .SingleOrDefaultAsync();

        public void CreateBank(Banks banks) => Create(banks);
        
    }
}