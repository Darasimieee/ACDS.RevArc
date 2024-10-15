using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IOrganisationService
	{
        Task<(IEnumerable<GetOrganisationDto> organisations, MetaData metaData)> GetAllOrganisationsAsync(DefaultParameters requestParameters, bool trackChanges);
        Task<List<GetOrganisationDto1>> GetAllOrganisationsWithoutImagesAsync(PaginationFilter filter);
        Task<GetOrganisationDto> GetOrganisationAsync(int id, bool trackChanges);
        Task<Response> CreateOrganisationAsync(CreateOrganisationDto organisation);
        Task<Response> UpdateOrganisationAsync(int id, UpdateOrganisationDto updateOrganisation, bool trackChanges);
        Task<IEnumerable<GetOrganisationDto>> ApprovedOnboardingRequestsAsync();
        Task<IEnumerable<GetOrganisationDto>> RejectedOnboardingRequestsAsync();
        Task<IEnumerable<GetOrganisationDto>> PendingOnboardingRequestsAsync();
        Task DeactivateOrganisation(int id);
        Task ActivateOrganisation(int id);
        Task<Response> ApproveOnboardingRequestAsync(int id, string agencycode);
        Task RejectOnboardingRequestAsync(int id);
        Task<List<GetTenancyDto>> GetAllOrganisationTenantsAsync(PaginationFilter filter);
        Task<List<GetTenancyDto>> GetOrganisationTenantAsync(int tenantId);
        Task<Response> UpdateOrganisationTenantAsync(int tenantId, UpdateTenancyDto updateTenant);
    }
}