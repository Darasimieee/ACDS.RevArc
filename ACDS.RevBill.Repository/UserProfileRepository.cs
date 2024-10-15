using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class UserProfileRepository : RepositoryBase<UserProfiles>, IUserProfileRepository
    {
        public UserProfileRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<UserProfiles>> GetAllUserProfilesAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            var userProfiles = await FindByCondition(e => e.OrganisationId.Equals(organisationId), trackChanges)
             .OrderBy(e => e.UserProfileId)
             .Include(o => o.Organisation)
             .Include(o => o.UserRoles)
             .ToListAsync();

            return PagedList<UserProfiles>
                .ToPagedList(userProfiles, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<UserProfiles> GetUserProfileAsync(int organisationId, int userProfileId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.UserProfileId.Equals(userProfileId), trackChanges)
            .Include(o => o.Organisation)
            .Include(o => o.UserRoles)
            .SingleOrDefaultAsync();

        public void CreateUserProfileForOrganisation(int organisationId, int userId, int userRoleId, UserProfiles userProfiles,bool ishead)
        {
            userProfiles.UserId = userId;
            userProfiles.OrganisationId = organisationId;
            userProfiles.UserRoleId = userRoleId;
            userProfiles.IsHead = ishead;
            Create(userProfiles);
        }
    }
}

