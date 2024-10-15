using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Auth;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IUserService
	{
        Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersInOrganisationAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<UserDto> GetUserInOrganisationAsync(int organisationId, int userId, bool trackChanges);
        Task<Response> CreateUserForOrganisationAsync(int organisationId, UserRequestParams user, bool trackChanges);
        Task UpdateUserAsync(int organisationId, int userId, UserUpdateDto userUpdate, bool trackOrgChanges, bool trackUserChanges);
        Task<Response> UpdatePasswordAsync(UpdatePasswordParams user, bool trackOrgChanges, bool trackUserPasswordChanges);
        Task<Response> ForgotPasswordAsync(OTPRequest model);
        Task<Response> VerifyForgotPasswordOTPAsync(ValidateOTPRequest model);
        Task<Response> ConfirmAccount(ValidateOTPRequest model);
        Task GenerateConfirmAccountOTP(OTPRequest model);
        Task<List<UserProfileDto>> GetAllUserProfilesAsync(int organisationId, PaginationFilter filter);
        Task<List<UserProfileDto>> GetAllUserProfilesbyAgencyAsync(int organisationId, int agencyId, PaginationFilter filter);
        Task<List<UserProfileDto>> GetUserProfileAsync(int organisationId, int userId);
        Task<Response> UpdateUserProfileAsync(int organisationId, int userId, UserProfileUpdateDto userProfileUpdate, bool trackOrgChanges, bool trackUserProfileChanges);
        Task<(IEnumerable<UserRoleDto> userRoles, MetaData metaData)> GetUserRolesAsync(int organisationId, DefaultParameters roleParameters, bool trackChanges);
        Task<UserRoleDto> GetUserRoleAsync(int organisationId, int userId, bool trackChanges);
        Task<Response> UpdateUserRoleAsync(int organisationId, int userId, UserRoleUpdateDto userRoleUpdate, bool trackOrgChanges, bool trackUserRoleChanges);
    }
}