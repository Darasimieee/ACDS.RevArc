using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IGenderRepository
	{
        Task<IEnumerable<Genders>> GetAllGendersAsync(bool trackChanges);
    }
}

