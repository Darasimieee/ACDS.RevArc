using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class MaritalStatusRepository : RepositoryBase<MaritalStatuses>, IMaritalStatusRepository
    {
        public MaritalStatusRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<MaritalStatuses>> GetAllMaritalStatusAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.MaritalStatusId)
                .ToListAsync();
    }
}

