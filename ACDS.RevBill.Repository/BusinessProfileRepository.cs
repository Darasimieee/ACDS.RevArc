using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ACDS.RevBill.Repository
{
    internal sealed class BusinessProfileRepository : RepositoryBase<BusinessProfile>, IBusinessProfileRepository
    {
        public BusinessProfileRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<BusinessProfile>> GetAllBusinessProfilesAsync(int OrganisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            var businessProfile = await FindByCondition(e => e.OrganisationId.Equals(OrganisationId), trackChanges)
               .OrderBy(e => e.BusinessProfileId)
               .Include(o => o.Property)
               .Include(o => o.BusinessType)
               .Include(o => o.BusinessSize)
               .Include(o => o.Revenues)
               .ToListAsync();

            return PagedList<BusinessProfile>
                .ToPagedList(businessProfile, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<BusinessProfile> GetBusinessProfileAsync(int OrganisationId, int BusinessProfileId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(OrganisationId) && c.BusinessProfileId.Equals(BusinessProfileId), trackChanges)
            .Include(o => o.Property)
            .Include(o => o.BusinessType)
            .Include(o => o.BusinessSize)
            .Include(o => o.Revenues)
            .SingleOrDefaultAsync();

        public void CreateBusinessProfile(int OrganisationId, int PropertyId, int CustomerId, BusinessProfile businessProfile)
        {
            businessProfile.OrganisationId = OrganisationId;
            businessProfile.PropertyId = PropertyId;
            businessProfile.CustomerId = CustomerId;
            Create(businessProfile);
        }

        public void CreateMultipleBusinessProfiles(int OrganisationId, int PropertyId, int CustomerId, IEnumerable<BusinessProfile> businessProfiles)
        {
            foreach (var businessProfile in businessProfiles)
            {
                businessProfile.OrganisationId = OrganisationId;
                businessProfile.PropertyId = PropertyId;
                businessProfile.CustomerId = CustomerId;
            }

            CreateMultiple(businessProfiles);
        }
    }
}

