using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface ISmsTemplateRepository
	{
        Task<PagedList<SmsTemplates>> GetTemplatesBySmsAccountId(int smsAccountId, RoleParameters roleParameters, bool trackChanges);
        Task<SmsTemplates> GetSmsTemplate(int smsAccountId, int smsTemplateId, bool trackChanges);
        void CreateSmsTemplate(SmsTemplates smsTemplates);
    }
}
  
