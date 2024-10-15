using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IFrequencyRepository
	{
        Task<IEnumerable<Frequencies>> GetAllFrequencyAsync(bool trackChanges);
    }
}