using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class  LgasRepository : RepositoryBase<Lgas>, ILgasRepository
    {
        public LgasRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Lgas>> GetAllLgasAsync(bool trackChanges) =>
             await FindAll(trackChanges)
                 .OrderBy(e => e.Id)
                 .ToListAsync();
    }
}
