using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface ITenancyRepository
	{
        Task<PagedList<Tenancy>> GetAllTenantsAsync(DefaultParameters requestParameters, bool trackChanges);
        Task<Tenancy> GetTenantByIdAsync(int tenantId, bool trackChanges);
        void CreateTenantForOrganisation(int organisationId, Tenancy tenancy);
    }
}