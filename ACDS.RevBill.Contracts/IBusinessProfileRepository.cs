using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IBusinessProfileRepository
	{
        Task<PagedList<BusinessProfile>> GetAllBusinessProfilesAsync(int OrganisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<BusinessProfile> GetBusinessProfileAsync(int OrganisationId, int BusinessProfileId, bool trackChanges);
        void CreateBusinessProfile(int OrganisationId, int PropertyId, int CustomerId, BusinessProfile businessProfile);
        void CreateMultipleBusinessProfiles(int OrganisationId, int PropertyId, int CustomerId, IEnumerable<BusinessProfile> businessProfiles);
    }
}