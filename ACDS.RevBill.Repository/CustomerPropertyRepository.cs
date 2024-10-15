using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class CustomerPropertyRepository : RepositoryBase<CustomerProperty>, ICustomerPropertyRepository
    {
        public CustomerPropertyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<CustomerProperty>> GetAllCustomersPropertiesAsync(int organisationId, int propertyId, DefaultParameters requestParameters, bool trackChanges)
        {
            var customers = await FindByCondition(e => e.OrganisationId.Equals(organisationId) && e.PropertyId.Equals(propertyId) && e.PropertyId != null, trackChanges)
               .OrderBy(e => e.CustomerPropertyId)
               .Include(o => o.Customers)
               .Include(o => o.Property)
               .ToListAsync();

            return PagedList<CustomerProperty>
                .ToPagedList(customers, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public void CreateCustomerProperty(int organisationId, int propertyId, int customerId, CustomerProperty customer)
        {
            customer.OrganisationId = organisationId;
            customer.CustomerId = customerId;
            customer.PropertyId = propertyId;
            Create(customer);
        }
    }
}