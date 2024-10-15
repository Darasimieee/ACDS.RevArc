using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACDS.RevBill.Services
{
    internal sealed class EmailTemplateService : IEmailTemplateService
    {   
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmailTemplateService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetEmailTemplateDto> emailTemplates, MetaData metaData)> GetTemplatesByAccountId(int emailAccountId, RoleParameters requestParameters, bool trackChanges)
        {
            var emailTemplateWithMetaData = await _repository.EmailTemplates.GetTemplatesByAccountId(emailAccountId, requestParameters, trackChanges);

            var emailTemplateDto = _mapper.Map<IEnumerable<GetEmailTemplateDto>>(emailTemplateWithMetaData);

            return (emailTemplate: emailTemplateDto, metaData: emailTemplateWithMetaData.MetaData);
        }

        public async Task<GetEmailTemplateDto> GetEmailTemplate(int emailAccountId,int emailTemplateId, bool trackChanges)
        {
            var emailTemplate = await _repository.EmailTemplates.GetEmailTemplate(emailAccountId,emailTemplateId, trackChanges);
            //check if the menu is null
            if (emailTemplate is null)
                throw new IdNotFoundException("email template", emailTemplateId);

            var emailTemplateDto = _mapper.Map<GetEmailTemplateDto>(emailTemplate);

            return emailTemplateDto;
        }

        public async Task<GetEmailTemplateDto> CreateEmailTemplate(int emailAccountId,CreateEmailTemplateDto createEmailTemplateDto)
        {
            var emailTemplateEntity = _mapper.Map<EmailTemplates>(createEmailTemplateDto);
            emailTemplateEntity.EmailAccountId= emailAccountId;
            _repository.EmailTemplates.CreateEmailTemplate(emailTemplateEntity);
            await _repository.SaveAsync();

            var emailTemplateToReturn = _mapper.Map<GetEmailTemplateDto>(emailTemplateEntity);

            return emailTemplateToReturn;
        }

        public async Task UpdateEmailTemplate(int emailAccountId, int emailTemplateId, UpdateEmailTemplateDto updateEmailTemplate, bool trackChanges)
        {
            var emailTemplateEntity = await _repository.EmailTemplates.GetEmailTemplate(emailAccountId,emailTemplateId, trackChanges);
            if (emailTemplateEntity is null)
                throw new IdNotFoundException("email template", emailTemplateId);

            _mapper.Map(updateEmailTemplate, emailTemplateEntity);
            await _repository.SaveAsync();
        }


    }
}
