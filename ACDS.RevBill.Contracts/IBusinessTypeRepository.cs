using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IBusinessTypeRepository
	{
        Task<IEnumerable<BusinessType>> GetAllBusinessTypesAsync(int organisationId, bool trackChanges);
        Task<BusinessType> GetBusinessTypeAsync(int organisationId, int businessTypeId, bool trackChanges);
        void CreateBusinessType(int organisationId, BusinessType businessType);
    }
}