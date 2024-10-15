using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IBusinessSizeRepository
	{
        Task<IEnumerable<BusinessSize>> GetAllBusinessSizesAsync(int organisationId, bool trackChanges);
        Task<BusinessSize> GetBusinessSizeAsync(int organisationId, int businessSizeId, bool trackChanges);
        void CreateBusinessSize(int organisationId, BusinessSize businessSize);
    }
}

