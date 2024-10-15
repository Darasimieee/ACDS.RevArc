using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Contracts
{
    public interface IStatesRepository
    {
        Task<IEnumerable<States>> GetAllStatesAsync(bool trackChanges);
    }
}