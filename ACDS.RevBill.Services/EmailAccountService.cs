using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;

namespace ACDS.RevBill.Services
{
    internal sealed class EmailAccountService : IEmailAccountService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmailAccountService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetEmailAccountDto> emailAccounts, MetaData metaData)> GetAllEmailAccounts(RoleParameters requestParameters, bool trackChanges)
        {
            var emailAccountWithMetaData = await _repository.EmailAccounts.GetAllEmailAccounts(requestParameters, trackChanges);

            var emailAccountDto = _mapper.Map<IEnumerable<GetEmailAccountDto>>(emailAccountWithMetaData);

            return (emailAccounts: emailAccountDto, metaData: emailAccountWithMetaData.MetaData);
        }

        public async Task<GetEmailAccountDto> GetEmailAccount(int emailAccountId, bool trackChanges)
        {
            var emailAccount = await _repository.EmailAccounts.GetEmailAccount(emailAccountId, trackChanges);
            //check if the menu is null
            if (emailAccount is null)
                throw new IdNotFoundException("email account", emailAccountId);

            var emailAccountDto = _mapper.Map<GetEmailAccountDto>(emailAccount);

            return emailAccountDto;
        }

        public async Task<GetEmailAccountDto> CreateEmailAccount(CreateEmailAccountDto createEmailAccounDto)
        {
            var emailAccountEntity = _mapper.Map<EmailAccounts>(createEmailAccounDto);
           
            _repository.EmailAccounts.CreateEmailAccount(emailAccountEntity);
            await _repository.SaveAsync();

            var emailAccountToReturn = _mapper.Map<GetEmailAccountDto>(emailAccountEntity);

            return emailAccountToReturn;
        }

        public async Task UpdateEmailAccount(int Id, UpdateEmailAccountDto updateEmailAccount, bool trackChanges)
        {
            var emailAccountEntity = await _repository.EmailAccounts.GetEmailAccount(Id, trackChanges);
            if (emailAccountEntity is null)
                throw new IdNotFoundException("email account", Id);

            _mapper.Map(updateEmailAccount, emailAccountEntity);
            await _repository.SaveAsync();
        }
    }
}
