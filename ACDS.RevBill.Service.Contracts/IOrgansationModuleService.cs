using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IOrganisationModuleService
	{
        Task<(IEnumerable<GetOrganisationModuleDto> organisationModules, MetaData metaData)> GetAllOrganisationModulesAsync(int organisationId, RoleParameters requestParameters, bool trackChanges);
        Task<GetOrganisationModuleDto> GetOrganisationModuleAsync(int moduleId, int organisationId, bool trackChanges);
        Task<Response> CreateOrganisationModuleAsync(int organisationId, List<CreateOrganisationModuleDto> orgModule);
    }
}