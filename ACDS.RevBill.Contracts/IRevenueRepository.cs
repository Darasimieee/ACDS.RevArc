using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IRevenueRepository
	{
        Task<PagedList<Revenues>> GetAllRevenuesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Revenues>> GetRevenuesbyOrgAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Revenues>> GetRevenuesbyBusinessTypeAsync(int organisationId, int businesstypeId, RoleParameters roleParameters, bool trackChanges);
        Task<Revenues> GetRevenuebyOrganisationId(int organisationId, string revenueName, bool trackChanges);

        Task<Revenues> GetRevenuebyOrganisationIdAndDescription(int organisationId, string revenueName, int businessTypeId, bool trackChanges);

        Task<Revenues> GetRevenueAsync(int Id, bool trackChanges);
        void CreateRevenueAsync(Revenues revenue);
    }
}

