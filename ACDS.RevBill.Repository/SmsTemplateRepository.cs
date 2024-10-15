using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Repository
{
    internal sealed class SmsTemplateRepository : RepositoryBase<SmsTemplates>, ISmsTemplateRepository
    {
        public SmsTemplateRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<PagedList<SmsTemplates>> GetTemplatesBySmsAccountId(int smsAccountId, RoleParameters requestParameters, bool trackChanges)
        {
            var smsTemplates = await FindAll(trackChanges)
                .OrderBy(e => e.SmsAccountId == smsAccountId)
                .ToListAsync();

            return PagedList<SmsTemplates>
                .ToPagedList(smsTemplates, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<SmsTemplates> GetSmsTemplate(int smsAccountId, int id, bool trackChanges) =>
            await FindByCondition(c => c.SmsTemplateId.Equals(id) && c.SmsAccountId == smsAccountId, trackChanges)
            .SingleOrDefaultAsync();

        public void CreateSmsTemplate(SmsTemplates smsTemplates) => Create(smsTemplates);

        // public void DeleteLga(Lgas lgas) => Delete(lgas);

    }
  
}
