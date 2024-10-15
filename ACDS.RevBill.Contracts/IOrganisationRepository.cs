using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IOrganisationRepository
	{
        Task<PagedList<Organisation>> GetAllOrganisationAsync(DefaultParameters requestParameters, bool trackChanges);
        Task<Organisation> GetOrganisationAsync(int Id, bool trackChanges);
        void CreateOrganisation(Organisation organisations);
        void DeleteOrganisation(Organisation organisations);
    }
}

