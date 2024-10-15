using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IUserProfileRepository
	{
        Task<PagedList<UserProfiles>> GetAllUserProfilesAsync(int urganisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<UserProfiles> GetUserProfileAsync(int organisationId, int userProfileId, bool trackChanges);
        void CreateUserProfileForOrganisation(int organisationId, int userId, int userProfileId, UserProfiles userProfiles, bool ishead);
    }
}

