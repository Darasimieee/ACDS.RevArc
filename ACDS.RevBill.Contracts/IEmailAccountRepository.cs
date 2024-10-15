using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IEmailAccountRepository
	{
        Task<PagedList<EmailAccounts>> GetAllEmailAccounts(RoleParameters roleParameters, bool trackChanges);
        Task<EmailAccounts> GetEmailAccount(int EmailAccountId, bool trackChanges);
        void CreateEmailAccount(EmailAccounts emailAccounts);
    }
}

