using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IAgencyService
    {
        Task<(IEnumerable<AgencyDto> agencies, MetaData metaData)> GetAllAgenciesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<AgencyDto> agencies, MetaData metaData)> GetOrganisationsAgencyAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<AgencyDto> GetAgencyAsync(int Id, bool trackChanges);
        Task<AgencyDto> GetAgencybyIdOrgId(int Id, int organisationId,bool trackChanges);      
        Task<AgencyDto> CreateAgencyAsync(AgencyCreationDto agency);
        Task<Response> FetchAgencyAsync(int organisationId);
        Task UpdateAgencyAsync(int Id, AgencyUpdateDto agencyUpdate, bool trackChanges);
    }
}

