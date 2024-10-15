using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class FrequencyRepository : RepositoryBase<Frequencies>, IFrequencyRepository
    {
        public FrequencyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Frequencies>> GetAllFrequencyAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.Id)
                .ToListAsync();
    }
}

