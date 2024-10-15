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
    internal sealed class EmailTemplateRepository : RepositoryBase<EmailTemplates>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<PagedList<EmailTemplates>> GetTemplatesByAccountId(int emailAccountId, RoleParameters requestParameters, bool trackChanges)
        {
            var emailTemplates = await FindAll(trackChanges)
                .OrderBy(e => e.EmailAccountId == emailAccountId)
                .ToListAsync();

            return PagedList<EmailTemplates>
                .ToPagedList(emailTemplates, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<EmailTemplates> GetEmailTemplate(int emailAccountId, int id, bool trackChanges) =>
            await FindByCondition(c => c.EmailTemplateId.Equals(id) && c.EmailAccountId== emailAccountId, trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmailTemplate(EmailTemplates emailTemplates) => Create(emailTemplates);

        // public void DeleteLga(Lgas lgas) => Delete(lgas);

    }
  
}
