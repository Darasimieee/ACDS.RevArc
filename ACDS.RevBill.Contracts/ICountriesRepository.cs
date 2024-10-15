using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Contracts
{
	public interface ICountriesRepository
	{
        Task<IEnumerable<Countries>> GetAllCountriesAsync(bool trackChanges);
    }
}

