using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IUsersRepository
	{
        Task<PagedList<Users>> GetAllUsersAsync(int OrganisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<Users> GetUserAsync(int OrganisationId, int UserId, bool trackChanges);
        Task<Users> GetEmailAsync(int OrganisationId, string Email, bool trackChanges);
        Task<Users> GetUsernameAsync(int OrganisationId, string Username, bool trackChanges);
        void CreateUserForOrganisation(int OrganisationId, Users users);
    }
}

