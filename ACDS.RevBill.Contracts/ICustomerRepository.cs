using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
    public interface ICustomerRepository
    {
        Task<PagedList<Customers>> GetAllCustomersAsync(int organisationId, CustomerParameters requestParameters, bool trackChanges);
        Task<Customers> GetCustomerAsync(int organisationId, int customerId, bool trackChanges);
        Task<Customers> GetCustomerByEmailAsync(string email, bool trackChanges);
        void CreateCustomer(int organisationId, Customers customer);
    }
}