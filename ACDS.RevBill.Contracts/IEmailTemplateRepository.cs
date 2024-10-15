using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IEmailTemplateRepository
	{
        Task<PagedList<EmailTemplates>> GetTemplatesByAccountId(int emailAccountId, RoleParameters roleParameters, bool trackChanges);
        Task<EmailTemplates> GetEmailTemplate(int emailAccountId, int emailTemplateId, bool trackChanges);
        void CreateEmailTemplate(EmailTemplates emailTemplates);
    }
}

