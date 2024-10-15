using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class TenancyRepository : RepositoryBase<Tenancy>, ITenancyRepository
    {
        public TenancyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Tenancy>> GetAllTenantsAsync(DefaultParameters requestParameters, bool trackChanges)
        {
            var users = await FindAll(trackChanges)
           .OrderBy(e => e.Id)
           .Include(o => o.Organisation)
           .ToListAsync();

            return PagedList<Tenancy>
                .ToPagedList(users, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Tenancy> GetTenantByIdAsync(int tenantId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(tenantId), trackChanges)
            .Include(o => o.Organisation)
            .SingleOrDefaultAsync();

        public void CreateTenantForOrganisation(int organisationId, Tenancy tenancy)
        {
            tenancy.OrganisationId = organisationId;
            Create(tenancy);
        }
    }
}