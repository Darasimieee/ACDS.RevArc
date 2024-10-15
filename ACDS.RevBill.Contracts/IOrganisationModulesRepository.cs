using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IOrganisationModulesRepository
    {
        Task<PagedList<OrganisationModules>> GetAllOrganisationModulesAsync(int organisationId, RoleParameters requestParameters, bool trackChanges);
        Task<List<OrganisationModules>> GetOrgModuleAsync(int organisationId, bool trackChanges);
        Task<OrganisationModules> GetOrganisationModuleAsync(int module,int organisationId, bool trackChanges);
        Task<OrganisationModules> GetOrganisationIdModuleAsync(int organisationModuleId, bool trackChanges);  
        void CreateOrganisationModule(OrganisationModules organisationModules);
       
    }
}

