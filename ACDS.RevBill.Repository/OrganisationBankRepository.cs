using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class OrganisationBankRepository : RepositoryBase<OrganisationBanks>, IOrganisationBankRepository
    {
        public OrganisationBankRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<OrganisationBanks>> GetAllOrganisationBanksAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            var banks = await FindByCondition(e => e.OrganisationId.Equals(organisationId), trackChanges)
                       .OrderBy(e => e.OrganisationBankId)
                       .Include(e => e.Banks)
                       .ToListAsync();

            return PagedList<OrganisationBanks>
                .ToPagedList(banks, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<OrganisationBanks> GetOrganisationBankAsync(int organisationId, int organisationBankId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.OrganisationBankId.Equals(organisationBankId), trackChanges)
            .Include(e => e.Banks)
            .SingleOrDefaultAsync();


        public void CreateOrganisationBank(int organisationId, int bankId, OrganisationBanks organisationBanks)
        {
            organisationBanks.OrganisationId = organisationId;
            organisationBanks.BankId = bankId;
            Create(organisationBanks);
        }
    }
}