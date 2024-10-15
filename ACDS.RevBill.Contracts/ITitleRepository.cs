using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface ITitleRepository
	{
        Task<IEnumerable<Titles>> GetAllTitlesAsync(bool trackChanges);
    }
}

