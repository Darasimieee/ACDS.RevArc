using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IMaritalStatusRepository
	{
        Task<IEnumerable<MaritalStatuses>> GetAllMaritalStatusAsync(bool trackChanges);
    }
}

