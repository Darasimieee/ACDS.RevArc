using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class SpaceIdentifierRepository : RepositoryBase<SpaceIdentifier>, ISpaceIdentifierRepository
    {
        public SpaceIdentifierRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<SpaceIdentifier>> GetAllSpaceIdentifiersAsync(int organisationId, bool trackChanges) =>
           await FindByCondition(c => c.OrganisationId.Equals(organisationId), trackChanges)
               .OrderBy(e => e.Id)
               .ToListAsync();

        public async Task<SpaceIdentifier> GetSpaceIdentifierAsync(int organisationId, int spaceIdentifieId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.Id.Equals(spaceIdentifieId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateSpaceIdentifier(int organisationId, SpaceIdentifier spaceIdentifier)
        {
            spaceIdentifier.OrganisationId = organisationId;
            Create(spaceIdentifier);
        }
    }
}

