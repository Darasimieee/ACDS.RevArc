using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.SmsAccount;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACDS.RevBill.Services
{
    internal sealed class SmsTemplateService : ISmsTemplateService
    {   
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public SmsTemplateService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetSmsTemplateDto> SmsTemplates, MetaData metaData)> GetTemplatesBySmsAccountId(int smsAccountId, RoleParameters requestParameters, bool trackChanges)
        {
            var smsTemplateWithMetaData = await _repository.SmsTemplates.GetTemplatesBySmsAccountId(smsAccountId, requestParameters, trackChanges);

            var smsTemplateDto = _mapper.Map<IEnumerable<GetSmsTemplateDto>>(smsTemplateWithMetaData);

            return (smsTemplates: smsTemplateDto, metaData:smsTemplateWithMetaData.MetaData);
        }

        public async Task<GetSmsTemplateDto> GetSmsTemplate(int smsAccountId,int smsTemplateId, bool trackChanges)
        {
            var smsTemplate = await _repository.SmsTemplates.GetSmsTemplate(smsAccountId, smsTemplateId, trackChanges);
            //check if the menu is null
            if (smsTemplate is null)
                throw new IdNotFoundException("sms template", smsTemplateId);

            var smsTemplateDto = _mapper.Map<GetSmsTemplateDto>(smsTemplate);

            return smsTemplateDto;
        }

        public async Task<GetSmsTemplateDto> CreateSmsTemplate(int smsAccountId,CreateSmsTemplateDto createSmsTemplateDto)
        {
            var smsTemplateEntity = _mapper.Map<SmsTemplates>(createSmsTemplateDto);
            smsTemplateEntity.SmsAccountId= smsAccountId;
            _repository.SmsTemplates.CreateSmsTemplate(smsTemplateEntity);
            await _repository.SaveAsync();

            var smsTemplateToReturn = _mapper.Map<GetSmsTemplateDto>(smsTemplateEntity);

            return smsTemplateToReturn;
        }

        public async Task UpdateSmsTemplate(int smsAccountId, int smsTemplateId, UpdateSmsTemplateDto updateSmsTemplate, bool trackChanges)
        {
            var smsTemplateEntity = await _repository.EmailTemplates.GetEmailTemplate(smsAccountId,smsTemplateId, trackChanges);
            if (smsTemplateEntity is null)
                throw new IdNotFoundException("sms template", smsTemplateId);

            _mapper.Map(updateSmsTemplate, smsTemplateEntity);
            await _repository.SaveAsync();
        }


    }
}