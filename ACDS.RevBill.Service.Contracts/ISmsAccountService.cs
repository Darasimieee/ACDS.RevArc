using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.SmsAccount;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface ISmsAccountService
    {
        Task<(IEnumerable<GetSmsAccountDto> smsAccounts, MetaData metaData)> GetAllSmsAccounts(RoleParameters roleParameters, bool trackChanges);
        Task<GetSmsAccountDto> GetSmsAccount(int Id, bool trackChanges);
        Task<GetSmsAccountDto> CreateSmsAccount(CreateSmsAccountDto smsAccount);
        Task UpdateSmsAccount(int Id, UpdateSmsAccountDto updateSmsAccount, bool trackChanges);
    }
}
