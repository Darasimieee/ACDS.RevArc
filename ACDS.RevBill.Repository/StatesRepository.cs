using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class StatesRepository : RepositoryBase<States>, IStatesRepository
    {
        public StatesRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<States>> GetAllStatesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.Id)
                .ToListAsync();
    }
}

