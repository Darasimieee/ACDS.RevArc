using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
    public interface ICustomerPropertyRepository
    {
        Task<PagedList<CustomerProperty>> GetAllCustomersPropertiesAsync(int organisationId, int propertyId, DefaultParameters requestParameters, bool trackChanges);
        void CreateCustomerProperty(int organisationId, int propertyId, int customerId, CustomerProperty customer);
    }
}