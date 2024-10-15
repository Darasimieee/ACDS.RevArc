using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.SmsAccount;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;

namespace ACDS.RevBill.Services
{
    internal sealed class SmsAccountService : ISmsAccountService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public SmsAccountService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetSmsAccountDto> smsAccounts, MetaData metaData)> GetAllSmsAccounts(RoleParameters requestParameters, bool trackChanges)
        {
            var smsAccountWithMetaData = await _repository.SmsAccounts.GetAllSmsAccounts(requestParameters, trackChanges);

            var smsAccountDto = _mapper.Map<IEnumerable<GetSmsAccountDto>>(smsAccountWithMetaData);

            return (smsAccounts: smsAccountDto, metaData: smsAccountWithMetaData.MetaData);
        }

        public async Task<GetSmsAccountDto> GetSmsAccount(int smsAccountId, bool trackChanges)
        {
            var smsAccount = await _repository.SmsAccounts.GetSmsAccount(smsAccountId, trackChanges);
            //check if the menu is null
            if (smsAccount is null)
                throw new IdNotFoundException("sms account", smsAccountId);

            var smsAccountDto = _mapper.Map<GetSmsAccountDto>(smsAccount);

            return smsAccountDto;
        }

        public async Task<GetSmsAccountDto> CreateSmsAccount(CreateSmsAccountDto createSmsAccounDto)
        {
            var smsAccountEntity = _mapper.Map<SmsAccounts>(createSmsAccounDto);
           
            _repository.SmsAccounts.CreateSmsAccount(smsAccountEntity);
            await _repository.SaveAsync();

            var smsAccountToReturn = _mapper.Map<GetSmsAccountDto>(smsAccountEntity);

            return smsAccountToReturn;
        }

        public async Task UpdateSmsAccount(int Id, UpdateSmsAccountDto updateSmsAccount, bool trackChanges)
        {
            var smsAccountEntity = await _repository.SmsAccounts.GetSmsAccount(Id, trackChanges);
            if (smsAccountEntity is null)
                throw new IdNotFoundException("sms account", Id);

            _mapper.Map(updateSmsAccount, smsAccountEntity);
            await _repository.SaveAsync();
        }


    }
}
