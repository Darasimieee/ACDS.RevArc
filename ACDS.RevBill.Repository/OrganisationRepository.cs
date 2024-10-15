using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class OrganisationRepository: RepositoryBase<Organisation>, IOrganisationRepository
    {
        public OrganisationRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Organisation>> GetAllOrganisationAsync(DefaultParameters requestParameters, bool trackChanges)
        {
            var organisations = await FindAll(trackChanges)
                .OrderBy(e => e.OrganisationName)
                .ToListAsync();

            return PagedList<Organisation>
                .ToPagedList(organisations, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Organisation> GetOrganisationAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateOrganisation(Organisation organisations) => Create(organisations);

        public void DeleteOrganisation(Organisation organisations) => Delete(organisations);
    }   
}

