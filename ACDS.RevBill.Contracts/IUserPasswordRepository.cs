using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IUserPasswordRepository
	{
        Task<UserPasswords> GetUserPasswordAsync(int UserPasswordId, bool trackChanges);
        void AddUserPassword(int OrganisationId, int UserId, UserPasswords userPasswords);
    }
}

