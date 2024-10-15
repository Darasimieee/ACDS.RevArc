using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IHeadRevenueService
    {

        Task<IEnumerable<HeadRevenue>> GetRevenuesbyHeadAsync(int organisationId, bool trackChanges);
        void CreateHeadRevenueAsync(HeadRevenue headrevenue, bool trackChanges);
    }
}

      