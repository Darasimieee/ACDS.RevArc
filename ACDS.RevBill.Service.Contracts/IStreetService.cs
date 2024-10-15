using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Agencies;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IStreetService
    {
        Task<(IEnumerable<GetStreetDto> street, MetaData metaData)> GetAllStreetsAsync(RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<GetStreetDto> street, MetaData metaData)> GetOrganisationsStreetAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<GetStreetDto> street, MetaData metaData)> GetAgencyStreetAsync(int organisationId,int agencyId, RoleParameters roleParameters, bool trackChanges);
        Task<GetStreetDto> GetStreetAsync(int Id, bool trackChanges);
        Task<IEnumerable<GetStreetDto>> GetStreetbyAgencyIdOrgId(int agencyId, int organisationId,bool trackChanges);

        Task<Response> CreateStreetAsync(BulkStreetCreation street);
        Task<Response> UpdateStreetAsync(int Id, StreetUpdateDto streetUpdate, bool trackChanges);
        Task<Response> UploadStreetAsync(int organisationId, int agencyid,string creator, IFormFile file);
    }
}

