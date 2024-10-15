using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IOrganisationBankRepository
	{
        Task<PagedList<OrganisationBanks>> GetAllOrganisationBanksAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<OrganisationBanks> GetOrganisationBankAsync(int organisationId, int organisationBankId, bool trackChanges);
        void CreateOrganisationBank(int organisationId, int bankId, OrganisationBanks organisationBanks);
    }
}