using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface ISmsAccountRepository
	{
        Task<PagedList<SmsAccounts>> GetAllSmsAccounts(RoleParameters roleParameters, bool trackChanges);
        Task<SmsAccounts> GetSmsAccount(int SmsAccountId, bool trackChanges);
        void CreateSmsAccount(SmsAccounts smsAccounts);
    }
}

