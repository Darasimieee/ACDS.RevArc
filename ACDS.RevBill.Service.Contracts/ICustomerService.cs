using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Auth;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface ICustomerService
	{
        Task<(IEnumerable<GetCustomerDto> customer, MetaData metaData)> GetAllCustomersAsync(int organisationId, CustomerParameters requestParameters, bool trackChanges);
        Task<GetCustomerDto> GetCustomerAsync(int organisationId, int customerId, bool trackChanges);
        Task<GetCustomerDto> GetCustomerByEmailAsync(string email, bool trackChanges);
        Task<Response> CreateCustomerWithoutPropertyAsync(int organisationId, CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(int organisationId, int customerId, UpdateCustomerDto updateCustomerDto, bool trackChanges);
        Task<Response> RegisterCustomerAsync(RegisterRequest model, bool trackChanges);
    }
}