using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IEmailTemplateService
    {
        Task<(IEnumerable<GetEmailTemplateDto> emailTemplates, MetaData metaData)> GetTemplatesByAccountId(int emailAccountId, RoleParameters roleParameters, bool trackChanges);
        Task<GetEmailTemplateDto> GetEmailTemplate(int emailAccountId, int emailTemplateId, bool trackChanges);
        Task<GetEmailTemplateDto> CreateEmailTemplate(int emailAccountId, CreateEmailTemplateDto createEmailTemplateDto);
        Task UpdateEmailTemplate(int emailAccountId, int emailTemplateId, UpdateEmailTemplateDto updateEmailTemplate, bool trackChanges);
    }
}
