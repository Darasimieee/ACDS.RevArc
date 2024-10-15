using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared; 
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IPropertyRepository
	{
        Task<PagedList<Property>> GetAllPropertiesAsync(int OrganisationId, PropertyParameters requestParameters, bool trackChanges);
        Task<PagedList<Property>> GetPropertiesbyAgencyAsync(int OrganisationId,int agencyId, PropertyParameters requestParameters, bool trackChanges);
        Task<Property> GetPropertyAsync(int OrganisationId, int PropertyId, bool trackChanges);
        void CreateProperty(int OrganisationId, Property property);
    }
}