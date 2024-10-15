using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class PayerTypeRepository : RepositoryBase<PayerTypes>, IPayerTypeRepository
    {
        public PayerTypeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<PayerTypes>> GetAllPayerTypesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.PayerTypeId)
                .ToListAsync();   
    }
}

