using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IHeadRevenueRepository
    {
        Task<IEnumerable<HeadRevenue>> GetRevenuesbyHeadAsync(int  organisationId, bool trackChanges);
        void CreateRevenueAsync(HeadRevenue headrevenues);
    }
}

