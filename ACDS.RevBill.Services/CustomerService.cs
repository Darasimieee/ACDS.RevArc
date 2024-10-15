using System.Data;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Email;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Auth;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;

namespace ACDS.RevBill.Services
{
    internal sealed class CustomerService : ICustomerService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private IMailService _mailService;
        private DataContext _context;
        private string? Password { get; set; }

        public CustomerService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IMailService mailService,
            DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _mailService = mailService;
            _context = context;
        }

        public async Task<(IEnumerable<GetCustomerDto> customer, MetaData metaData)> GetAllCustomersAsync(int organisationId, CustomerParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var customersWithMetaData = await _repository.Customer.GetAllCustomersAsync(organisationId, requestParameters, trackChanges);

            var customerDto = _mapper.Map<IEnumerable<GetCustomerDto>>(customersWithMetaData);

            return (customer: customerDto, metaData: customersWithMetaData.MetaData);
        }

        public async Task<GetCustomerDto> GetCustomerAsync(int organisationId, int customerId, bool trackChanges)
        {
            var customer = await _repository.Customer.GetCustomerAsync(organisationId, customerId, trackChanges);
            //check if the role is null
            if (customer is null)
                throw new IdNotFoundException("customer", customerId);

            var customerDto = _mapper.Map<GetCustomerDto>(customer);

            return customerDto;
        }

        public async Task<GetCustomerDto> GetCustomerByEmailAsync(string email, bool trackChanges)
        {
            var customer = await _repository.Customer.GetCustomerByEmailAsync(email, trackChanges);
            //check if the customer is null
            if (customer is null)
                throw new CustomerEmailNotFoundException(email);

            var customerDto = _mapper.Map<GetCustomerDto>(customer);

            return customerDto;
        }

        public async Task<Response> CreateCustomerWithoutPropertyAsync(int organisationId, CreateCustomerDto createCustomerDto)
        {
            Response response = new Response();
            var customerEntity = _mapper.Map<Customers>(createCustomerDto);
            var customerExist = await CheckCustomerExist(organisationId,createCustomerDto);
           if(customerExist!=0 )
            {
                response.Status = 409;
                response.StatusMessage = "Customer already Exists!";
                return response;   
            }
            //concatenate to get full name
            customerEntity.FullName = $"{customerEntity.FirstName} {customerEntity.MiddleName} {customerEntity.LastName}";

            _repository.Customer.CreateCustomer(organisationId, customerEntity);
            await _repository.SaveAsync();

            var customerToReturn = _mapper.Map<GetCustomerDto>(customerEntity);
            response.Status = 200;
            response.Data = customerToReturn;   
            response.StatusMessage = "Customer successfully created";
            return response;
        }
        private async Task<int> CheckCustomerExist(int organisationId, CreateCustomerDto createCustomerDto)
        {
            //concatenate to get full name
            var fullName = $"{createCustomerDto.FirstName} {createCustomerDto.MiddleName} {createCustomerDto.LastName}";

            var customer = await _context.Customers.Where(x => x.FullName.Equals(fullName) && ( x.Email.Equals(createCustomerDto.Email) || x.PhoneNo.Equals(createCustomerDto.PhoneNo)) && x.OrganisationId.Equals(organisationId)).SingleOrDefaultAsync();
            if (customer != null)
                return customer.CustomerId;
            return 0;
        }
        public async Task UpdateCustomerAsync(int organisationId, int customerId, UpdateCustomerDto updateCustomerDto, bool trackChanges)
        {
            var customerEntity = await _repository.Customer.GetCustomerAsync(organisationId, customerId, trackChanges);
            if (customerEntity is null)
                throw new IdNotFoundException("customer", customerId);

            _mapper.Map(updateCustomerDto, customerEntity);
            await _repository.SaveAsync();
        }

        public async Task<Response> RegisterCustomerAsync(RegisterRequest model, bool trackChanges)
        {
            Response dataResponse = new Response();

            var companyEmail = await _context.Customers.Where(x => x.Email.Equals(model.Email)).Select(x => x.Email).FirstOrDefaultAsync();
            var companyPhone = await _context.Customers.Where(u => u.PhoneNo.Equals(model.PhoneNumber)).Select(x => x.PhoneNo).FirstOrDefaultAsync();

           // check if user exists in User table
           if (await _context.Users.AnyAsync(x => x.Email == model.Email))
           {
                dataResponse.StatusMessage = $"Email'" + model.Email + "' already exists as a user";
                dataResponse.Status = 400;
           }

            //check if user exists in Customer table
            else if (companyEmail == null || companyPhone == null)
            {
                dataResponse.StatusMessage = "You have not been profiled. Kindly contact the admin for assistance.";
                dataResponse.Status = 400;
            }

            else
            {
                //get records from user customer and organisation table
                var customer = await _context.Customers.Where(u => u.Email.Equals(model.Email)).Select(x => x).FirstOrDefaultAsync();
                var organisationTenantName = await _context.Organisations
                    .Where(x => x.OrganisationId.Equals(customer.OrganisationId))
                    .Select(x => x.TenantName)
                    .FirstOrDefaultAsync();

                var organisationName = await _context.Organisations
                    .Where(x => x.OrganisationId.Equals(customer.OrganisationId))
                    .Select(x => x.OrganisationName)
                    .FirstOrDefaultAsync();

                var firstName = customer.FirstName;
                var lastName = customer.LastName;
                var middleName = customer.MiddleName;
                var organisationId = customer.OrganisationId;
                var username = customer.Email;
                var tenantName = organisationTenantName;

                //map user
                UserCreationDto userCreationDto = new();
                userCreationDto.UserName = username;
                userCreationDto.Email = model.Email;
                userCreationDto.PhoneNumber = model.PhoneNumber;
                userCreationDto.AccountConfirmed = false;
                userCreationDto.LockoutEnabled = true;
                userCreationDto.DateCreated = DateTime.Now;
                userCreationDto.CreatedBy = model.Email;
                userCreationDto.Active = true;
                userCreationDto.TenantName = tenantName;

                var userEntity = _mapper.Map<Users>(userCreationDto);

                //map user role
                UserRoleCreationDto userRoleCreationDto = new();
                userRoleCreationDto.RoleId = 4;
                userRoleCreationDto.CreatedBy = model.Email;
                userRoleCreationDto.DateCreated = DateTime.Now;
                userRoleCreationDto.TenantName = tenantName;

                var userRoleEntity = _mapper.Map<UserRoles>(userRoleCreationDto);
                //check for creators agency type head or sub
                var createdbytype = _context.UserProfiles.Where(x => x.Email.Equals(model.Email)).FirstOrDefault().IsHead;

                //map user profile
                UserProfileCreationDto userProfileCreationDto = new();
                userProfileCreationDto.Firstname = firstName;
                userProfileCreationDto.MiddleName = middleName;
                userProfileCreationDto.Surname = lastName;
                userProfileCreationDto.Email = model.Email;
                userProfileCreationDto.PhoneNumber = model.PhoneNumber;
                userProfileCreationDto.DateCreated = DateTime.Now;
                userProfileCreationDto.CreatedBy = model.Email;
                userProfileCreationDto.Active = true;
                userProfileCreationDto.UserRoleId = userRoleEntity.UserRoleId;
                userProfileCreationDto.TenantName = tenantName;

                var userProfileEntity = _mapper.Map<UserProfiles>(userProfileCreationDto);

                //map user password
                UserPasswordCreationDto userPasswordCreationDto = new();
                userPasswordCreationDto.Password = model.Password;
                userPasswordCreationDto.CreatedBy = model.Email;
                userPasswordCreationDto.TenantName = tenantName;
                userPasswordCreationDto.DateCreated = DateTime.Now;

                var userPasswordEntity = _mapper.Map<UserPasswords>(userPasswordCreationDto);

                //map password history table
                PasswordHistoryCreationDto passwordHistoryCreationDto = new();
                passwordHistoryCreationDto.Password = model.Password;
                passwordHistoryCreationDto.DateCreated = DateTime.Now;
                passwordHistoryCreationDto.CreatedBy = model.Email;

                var passwordHistoryEntity = _mapper.Map<PasswordHistory>(passwordHistoryCreationDto);

                //store password in variable to send to user
                Password = userPasswordEntity.Password;

                //hash password 
                userPasswordEntity.Password = BCrypt.Net.BCrypt.HashPassword(userPasswordEntity.Password);
                passwordHistoryEntity.Password = userPasswordEntity.Password;

                //save to user table and customer table
                _repository.Users.CreateUserForOrganisation(organisationId, userEntity);

                //save to retrieve generated user id
                await _repository.SaveAsync();

                //save to user profile, user password and user role table
                _repository.UserRole.AddUserToRole(organisationId, userEntity.UserId, userRoleEntity);

                await _repository.SaveAsync();

                _repository.UserProfile.CreateUserProfileForOrganisation(organisationId, userEntity.UserId, userRoleEntity.UserRoleId, userProfileEntity, createdbytype);
                _repository.UserPassword.AddUserPassword(organisationId, userEntity.UserId, userPasswordEntity);
                _repository.PasswordHistory.AddPassword(userEntity.UserId, passwordHistoryEntity);

                await _repository.SaveAsync();

                //generate otp for account activation
                var pwd = new Password(6).IncludeNumeric();
                var otp = pwd.Next();

                AccountActivation accountActivation = new AccountActivation
                {
                    UserId = userEntity.UserId,
                    RequestTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddDays(1), //set otp to expire after a day
                    OTP = otp,
                    DateCreated = DateTime.Now,
                    CreatedBy = userCreationDto.Email
                };

                //save otp to database
                await _context.AccountActivation.AddAsync(accountActivation);
                await _context.SaveChangesAsync();

                //send activation email to user
                MailRequest mailRequest = new MailRequest();
                mailRequest.Subject = "RevBill Account Activation";
                mailRequest.ToEmail = userCreationDto.Email;
                mailRequest.Password = Password;
                mailRequest.OTP = otp;
                mailRequest.FirstName = userProfileEntity.Firstname;
                mailRequest.LastName = userProfileEntity.Surname;
                mailRequest.OrganisationName = organisationName;

                await _mailService.SendWelcomeEmailCustomerAsync(mailRequest);

                //send sms for otp
                MessageQueue messageQueue = new MessageQueue();
                messageQueue.Phone = model.PhoneNumber;
                messageQueue.Message = $"Kindly confirm your Revbill account - {model.Email} using this OTP - {otp}. It is valid for one day.";

                //response message
                dataResponse.StatusMessage = "Customer successfully registered";
                dataResponse.Status = 200;
            }

            return dataResponse;
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int organisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(organisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", organisationId);
        }
    }
}