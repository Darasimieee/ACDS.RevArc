using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class BusinessTypeRepository : RepositoryBase<BusinessType>, IBusinessTypeRepository
    {
        public BusinessTypeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<BusinessType>> GetAllBusinessTypesAsync(int organisationId, bool trackChanges) =>
           await FindByCondition(c => c.OrganisationId.Equals(organisationId), trackChanges)
               .OrderBy(e => e.Id)
               .ToListAsync();

        public async Task<BusinessType> GetBusinessTypeAsync(int organisationId, int businessTypeId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.Id.Equals(businessTypeId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateBusinessType(int organisationId, BusinessType businessType)
        {
            businessType.OrganisationId = organisationId;
            Create(businessType);
        }
    }
}

