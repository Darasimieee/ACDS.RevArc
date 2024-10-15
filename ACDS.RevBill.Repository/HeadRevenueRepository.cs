using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class HeadRevenueRepository : RepositoryBase<HeadRevenue>, IHeadRevenueRepository
    {
        public HeadRevenueRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<HeadRevenue>> GetRevenuesbyHeadAsync(int organisationId, bool trackChanges) =>
            await FindByCondition(x => x.OrganisationId.Equals(organisationId), trackChanges)
                .OrderBy(e => e.HeadRevenueId)
                .ToListAsync();
        public void CreateRevenueAsync(HeadRevenue headrevenues) => Create(headrevenues);

    }
}

