using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Contracts
{
	public interface ILcdaRepository
	{
        Task<IEnumerable<Lcdas>> GetAllLcdasAsync(bool trackChanges);
    }
}

