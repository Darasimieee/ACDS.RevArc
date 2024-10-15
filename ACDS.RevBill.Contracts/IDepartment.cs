using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool trackChanges);
    }
}

