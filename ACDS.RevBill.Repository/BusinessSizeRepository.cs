using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class BusinessSizeRepository : RepositoryBase<BusinessSize>, IBusinessSizeRepository
    {
        public BusinessSizeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<BusinessSize>> GetAllBusinessSizesAsync(int organisationId, bool trackChanges) =>
           await FindByCondition(c => c.OrganisationId.Equals(organisationId), trackChanges)
               .OrderBy(e => e.Id)
               .ToListAsync();

        public async Task<BusinessSize> GetBusinessSizeAsync(int organisationId, int businessSizeId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.Id.Equals(businessSizeId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateBusinessSize(int organisationId, BusinessSize businessSize)
        {
            businessSize.OrganisationId = organisationId;
            Create(businessSize);
        }
    }
}

