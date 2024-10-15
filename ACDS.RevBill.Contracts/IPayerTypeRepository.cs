using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IPayerTypeRepository
	{
        Task<IEnumerable<PayerTypes>> GetAllPayerTypesAsync(bool trackChanges);
    }
}

