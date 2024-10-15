using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.SmsAccount;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface ISmsTemplateService
    {
        Task<(IEnumerable<GetSmsTemplateDto> SmsTemplates, MetaData metaData)> GetTemplatesBySmsAccountId(int smsAccountId, RoleParameters roleParameters, bool trackChanges);
        Task<GetSmsTemplateDto> GetSmsTemplate(int smsAccountId, int smsTemplateId, bool trackChanges);
        Task<GetSmsTemplateDto> CreateSmsTemplate(int smsAccountId, CreateSmsTemplateDto createSmsTemplateDto);
        Task UpdateSmsTemplate(int smsAccountId, int smsTemplateId, UpdateSmsTemplateDto updateSmsTemplate, bool trackChanges);
    }
}
