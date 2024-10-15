using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class CountriesRepository : RepositoryBase<Countries>, ICountriesRepository
    {
        public CountriesRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Countries>> GetAllCountriesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.Id)
                .ToListAsync();
    }
}

