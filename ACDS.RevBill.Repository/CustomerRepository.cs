using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using ACDS.RevBill.Repository.Extensions;

namespace ACDS.RevBill.Repository
{
    internal sealed class CustomerRepository : RepositoryBase<Customers>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Customers>> GetAllCustomersAsync(int organisationId, CustomerParameters requestParameters, bool trackChanges)
        {
            var customers = await FindByCondition(e => e.OrganisationId.Equals(organisationId), trackChanges)
               .OrderBy(e => e.CustomerId)
               .SearchByEmail(requestParameters.Email)
               .SearchByPayerID(requestParameters.PayerId)
               .SearchByPhoneNumber(requestParameters.PhoneNumber)
               .SearchByName(requestParameters.Name)
               .Include(o => o.PayerTypes)
               .Include(o => o.Titles)
               .Include(o => o.Genders)
               .Include(o => o.MaritalStatuses)
               .ToListAsync();

            return PagedList<Customers>
                .ToPagedList(customers, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Customers> GetCustomerAsync(int organisationId, int customerId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.CustomerId.Equals(customerId), trackChanges)
            .Include(o => o.PayerTypes)
            .Include(o => o.Titles)
            .Include(o => o.Genders)
            .Include(o => o.MaritalStatuses)
            .SingleOrDefaultAsync();

        public async Task<Customers> GetCustomerByEmailAsync(string email, bool trackChanges) =>
            await FindByCondition(c => c.Email.Equals(email), trackChanges)
            .Include(o => o.PayerTypes)
            .Include(o => o.Titles)
            .Include(o => o.Genders)
            .Include(o => o.MaritalStatuses)
            .SingleOrDefaultAsync();

        public void CreateCustomer(int organisationId, Customers customer)
        {
            customer.OrganisationId = organisationId;
            Create(customer);
        }
    }
}