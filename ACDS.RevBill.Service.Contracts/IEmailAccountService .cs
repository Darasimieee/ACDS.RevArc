using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IEmailAccountService
    {
        Task<(IEnumerable<GetEmailAccountDto> emailAccounts, MetaData metaData)> GetAllEmailAccounts(RoleParameters roleParameters, bool trackChanges);
        Task<GetEmailAccountDto> GetEmailAccount(int Id, bool trackChanges);
        Task<GetEmailAccountDto> CreateEmailAccount(CreateEmailAccountDto emailAccount);
        Task UpdateEmailAccount(int Id, UpdateEmailAccountDto updateEmailAccount, bool trackChanges);
    }
}
