using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class TitleRepository : RepositoryBase<Titles>, ITitleRepository
    {
        public TitleRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Titles>> GetAllTitlesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.TitleId)
                .ToListAsync();
    }
}

