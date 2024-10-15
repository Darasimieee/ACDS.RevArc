using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IRevenueService
    {
        Task<(IEnumerable<RevenueDto> revenues, MetaData metaData)> GetAllRevenuesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<IEnumerable<HeadRevenue>> GetRevenuesbyHeadAsync(int organisationId, bool trackChanges);
        Task<Response> CreateheadRevenuesAsync(int organisationId, bool trackChanges);
        Task<(IEnumerable<RevenueDto> revenues, MetaData metaData)> GetOrganisationsRevenueAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<RevenueDto> revenues, MetaData metaData)> GetRevenuesbyBusinessTypeAsync(int organisationId, int businessTypeId, RoleParameters roleParameters, bool trackChanges);
        Task<RevenueDto> GetRevenueAsync(int Id, bool trackChanges);
       
        Task<Response> CreateRevenueAsync(RevenueCreationDto revenue);
        Task UpdateRevenueAsync(int Id, RevenueUpdateDto revenueUpdate, bool trackChanges);
    }
}

      