using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class GenderRepository : RepositoryBase<Genders>, IGenderRepository
    {
        public GenderRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Genders>> GetAllGendersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.GenderId)
                .ToListAsync();
    }
}

