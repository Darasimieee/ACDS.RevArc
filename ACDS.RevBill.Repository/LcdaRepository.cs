using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class LcdaRepository : RepositoryBase<Lcdas>, ILcdaRepository
    {
        public LcdaRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Lcdas>> GetAllLcdasAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(e => e.Id)
                .ToListAsync();
    }
}

