using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IBankRepository
	{
        Task<PagedList<Banks>> GetAllBanksAsync(DefaultParameters requestParameters, bool trackChanges);
        Task<Banks> GetBankAsync(int bankId, bool trackChanges);
        void CreateBank(Banks banks);
    }
}