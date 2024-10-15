using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Email;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PasswordGenerator;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Services
{
    internal sealed class OrganisationService : IOrganisationService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private IMailService _mailService;
        private DataContext _context;
        public JsonModelService _modelService;
        private readonly int _maxFileSize = 300000; //in bytes 300kb
        private readonly string[] _extensions = { ".jpg", ".png", ".jpeg" };
        public string? LogoFileName { get; set; }
        public string? LogoFileExtension { get; set; }
        public string? LogoNewFileName { get; set; }
        public string? BackgroundImageFileName { get; set; }
        public string? BackgroundImageFileExtension { get; set; }
        public string? BackgroundImageNewFileName { get; set; }
        private string? Password { get; set; }
        public IEnumerable<VerifyPidResponseDto>? VerifyPid { get; set; }

        public OrganisationService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IMailService mailService,
            DataContext context, JsonModelService modelService, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _mailService = mailService;
            _context = context;
            _modelService = modelService;
            _configuration = configuration;
        }

        public async Task<(IEnumerable<GetOrganisationDto> organisations, MetaData metaData)> GetAllOrganisationsAsync(DefaultParameters requestParameters, bool trackChanges)
        {
            var organistaionWithMetaData = await _repository.Organisation.GetAllOrganisationAsync(requestParameters, trackChanges);

            var organisationDto = _mapper.Map<IEnumerable<GetOrganisationDto>>(organistaionWithMetaData);

            return (organisations: organisationDto, metaData: organistaionWithMetaData.MetaData);
        }

        public async Task<List<GetOrganisationDto1>> GetAllOrganisationsWithoutImagesAsync(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var summaryList = await _context.Organisations
              .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
              .Take(validFilter.PageSize)
              .Where(x => x.OrganisationApprovalStatus == 2)
              .Select(a => new
              {
                  Organisation = _context.Organisations.Where(p => p.OrganisationId == a.OrganisationId).FirstOrDefault()
              }).ToListAsync();

            List<GetOrganisationDto1> result = new List<GetOrganisationDto1>();

            //add to list
            foreach (var item in summaryList)
            {
                result.Add(new GetOrganisationDto1
                {
                    OrganisationId = item.Organisation.OrganisationId,
                    StateId = item.Organisation.StateId,
                    CountryId = item.Organisation.CountryId,
                    LgaId = item.Organisation.LgaId,
                    LcdaId = item.Organisation.LcdaId,
                    PayerId = item.Organisation.PayerId,
                    OrganisationName = item.Organisation.OrganisationName,
                    Address = item.Organisation.Address,
                    City = item.Organisation.City,
                    PhoneNo = item.Organisation.PhoneNo,
                    Email = item.Organisation.Email,
                    WebUrl = item.Organisation.WebUrl,
                    OrganisationApprovalStatus = item.Organisation.OrganisationApprovalStatus,
                    OrganisationStatus = (bool)item.Organisation.OrganisationStatus
                });
            }

            return result;
        }

        public async Task<IEnumerable<GetOrganisationDto>> ApprovedOnboardingRequestsAsync()
        {
            var organistaion = await _context.Organisations.Where(x => x.OrganisationApprovalStatus == (int)OrganisationApprovalStatusEnum.Approved).Select(x => x).ToListAsync();

            var organisationDto = _mapper.Map<IEnumerable<GetOrganisationDto>>(organistaion);

            return organisationDto;
        }

        public async Task<IEnumerable<GetOrganisationDto>> RejectedOnboardingRequestsAsync()
        {
            var organistaion = await _context.Organisations.Where(x => x.OrganisationApprovalStatus == (int)OrganisationApprovalStatusEnum.Rejected).Select(x => x).ToListAsync();

            var organisationDto = _mapper.Map<IEnumerable<GetOrganisationDto>>(organistaion);

            return organisationDto;
        }

        public async Task<IEnumerable<GetOrganisationDto>> PendingOnboardingRequestsAsync()
        {
            var organistaion = await _context.Organisations.Where(x => x.OrganisationApprovalStatus == (int)OrganisationApprovalStatusEnum.Pending).Select(x => x).ToListAsync();

            var organisationDto = _mapper.Map<IEnumerable<GetOrganisationDto>>(organistaion);

            return organisationDto;
        }

        public async Task<GetOrganisationDto> GetOrganisationAsync(int Id, bool trackChanges)
        {
            await CheckIfOrganisationExists(Id, trackChanges);

            var organisation = await _repository.Organisation.GetOrganisationAsync(Id, trackChanges);

            var organisationDto = _mapper.Map<GetOrganisationDto>(organisation);

            return organisationDto;
        }

        public async Task<Response> CreateOrganisationAsync(CreateOrganisationDto organisation)
        {
            Response dataResponse = new Response();

            var organisationEntity = _mapper.Map<Organisation>(organisation);

            var checkIfPayerIDExists = _context.Organisations.Any(c => c.PayerId == organisationEntity.PayerId);

            if (checkIfPayerIDExists)
            {
                dataResponse.StatusMessage = $"Organisation with PayerID: {organisationEntity.PayerId} already exists";
                dataResponse.Status = 400;
            }

            else
            {
                //use payerid as tenantid
                organisationEntity.TenantName = organisation.PayerId;

                organisationEntity.OrganisationApprovalStatus = (int)OrganisationApprovalStatusEnum.Pending;
                organisationEntity.OrganisationStatus = false;
                
                //create file attachment for logo
                if (organisation.Logo != null)
                {
                    if (organisation.Logo.Length > 0)
                    {
                        LogoFileName = Path.GetFileName(organisation.Logo.FileName);
                        LogoFileExtension = Path.GetExtension(LogoFileName);
                        LogoNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension);
                    }

                    if (organisation.Logo.Length > _maxFileSize)
                    {
                        dataResponse.StatusMessage = "Maximum allowed image size is 300kb";
                        dataResponse.Status = 400;

                        return dataResponse;
                    }

                    if (!_extensions.Contains(LogoFileExtension.ToLower()))
                    {
                        dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                        dataResponse.Status = 400;

                        return dataResponse;
                    }

                    //convert to file to memory stream 
                    using (var ms = new MemoryStream())
                    {
                        organisation.Logo.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        //map logo data
                        organisationEntity.LogoData = fileBytes;
                        organisationEntity.LogoName = organisation.Logo.FileName;
                    }
                }

                //create file attachment for background image
                if (organisation.BackgroundImage != null)
                {
                    if (organisation.BackgroundImage.Length > 0)
                    {
                        BackgroundImageFileName = Path.GetFileName(organisation.BackgroundImage.FileName);
                        BackgroundImageFileExtension = Path.GetExtension(BackgroundImageFileName);
                        BackgroundImageNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), BackgroundImageFileExtension);
                    }

                    if (organisation.BackgroundImage.Length > _maxFileSize)
                    {
                        dataResponse.StatusMessage = "Maximum allowed image size is 300kb";
                        dataResponse.Status = 400;

                        return dataResponse;
                    }

                    if (!_extensions.Contains(BackgroundImageFileExtension.ToLower()))
                    {
                        dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                        dataResponse.Status = 400;

                        return dataResponse;
                    }

                    //convert to file to memory stream 
                    using (var ms = new MemoryStream())
                    {
                        organisation.BackgroundImage.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        //map background image data
                        organisationEntity.BackgroundImagesData = fileBytes;
                        organisationEntity.BackgroundImagesName = organisation.BackgroundImage.FileName;
                    }
                }

                _repository.Organisation.CreateOrganisation(organisationEntity);
                await _repository.SaveAsync();

                //add organisation tenant to tenancy table
                CreateTenancyDto createTenancyDto  = new();
                createTenancyDto.TenantId = organisationEntity.PayerId;

                //get appsettings connection string
                var connString = _configuration.GetConnectionString("sqlConnection");
                createTenancyDto.ConnectionString = connString;
                createTenancyDto.DateCreated = DateTime.Now;
                createTenancyDto.CreatedBy = "SYSTEM";

                var tenancyEntity = _mapper.Map<Tenancy>(createTenancyDto);

                _repository.Tenancy.CreateTenantForOrganisation(organisationEntity.OrganisationId, tenancyEntity);
                await _repository.SaveAsync();

                //send activation email to organisation
                MailRequest message = new MailRequest();
                message.Subject = "Welcome to RevBill";
                message.ToEmail = organisationEntity.Email;
                message.OrganisationName = organisation.OrganisationName;

                await _mailService.SendOnboardingOrganisationEmailAsync(message);

                dataResponse.StatusMessage = "Organisation Created Successfully";
                dataResponse.Status = 200;
                dataResponse.Data = organisationEntity;
            }
           
            return dataResponse;
        }

        public async Task<Response> UpdateOrganisationAsync(int Id, UpdateOrganisationDto updateOrganisation, bool trackChanges)
        {
            Response dataResponse = new Response();

            var organisationEntity = await _repository.Organisation.GetOrganisationAsync(Id, trackChanges);
            if (organisationEntity is null)
                throw new IdNotFoundException("organisation", Id);

            //create file attachment for logo
            if (updateOrganisation.Logo != null)
            {
                if (updateOrganisation.Logo.Length > 0)
                {
                    LogoFileName = Path.GetFileName(updateOrganisation.Logo.FileName);
                    LogoFileExtension = Path.GetExtension(LogoFileName);
                    LogoNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension);
                }

                if (updateOrganisation.Logo.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 300kb";
                    dataResponse.Status = 400;
                }

                if (!_extensions.Contains(LogoFileExtension.ToLower()))
                {
                    dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                    dataResponse.Status = 400;
                }

                //convert to file to memory stream 
                using (var ms = new MemoryStream())
                {
                    updateOrganisation.Logo.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    organisationEntity.LogoData = fileBytes;
                    organisationEntity.LogoName = updateOrganisation.Logo.FileName;
                }
            }

            //create file attachment for background image
            if (updateOrganisation.BackgroundImage != null)
            {
                if (updateOrganisation.BackgroundImage.Length > 0)
                {
                    BackgroundImageFileName = Path.GetFileName(updateOrganisation.BackgroundImage.FileName);
                    BackgroundImageFileExtension = Path.GetExtension(BackgroundImageFileName);
                    BackgroundImageNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), BackgroundImageFileExtension);
                }

                if (updateOrganisation.BackgroundImage.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 300kb";
                    dataResponse.Status = 400;
                }

                if (!_extensions.Contains(BackgroundImageFileExtension.ToLower()))
                {
                    dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                    dataResponse.Status = 400;
                }

                //convert to file to memory stream 
                using (var ms = new MemoryStream())
                {
                    updateOrganisation.BackgroundImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map background image data
                    organisationEntity.BackgroundImagesData = fileBytes;
                    organisationEntity.BackgroundImagesName = updateOrganisation.BackgroundImage.FileName;
                }
            }

            _mapper.Map(updateOrganisation, organisationEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Organisation Successfully Updated";
            dataResponse.Status = 200;
            dataResponse.Data = organisationEntity;

            return dataResponse;
        }

        public async Task DeactivateOrganisation(int id)
        {
            var organisation = await _context.Organisations.Where(x => x.OrganisationId.Equals(id)).FirstOrDefaultAsync();

            if (organisation is null)
                throw new IdNotFoundException("organisation", id);

            organisation.OrganisationStatus = false;

            await _context.SaveChangesAsync();
        }

        public async Task ActivateOrganisation(int id)
        {
            var organisation = await _context.Organisations.Where(x => x.OrganisationId.Equals(id)).FirstOrDefaultAsync();

            if (organisation is null)
                throw new IdNotFoundException("organisation", id);

            organisation.OrganisationStatus = true;

            await _context.SaveChangesAsync();
        }

        public async Task<Response> ApproveOnboardingRequestAsync(int id, string agencycode)
        {
            Response response = new Response();
            var allOrgs = _context.Organisations.ToList();
            var organisation = await _context.Organisations.Where(x => x.OrganisationId.Equals(id)).FirstOrDefaultAsync();
            if (organisation.OrganisationApprovalStatus == 2){
                response.Status = 403;
                response.StatusMessage = "Organisation already approved";
            }else {
            var agencyId = 0;
           
            if (organisation is null)
                throw new IdNotFoundException("organisation", id);
                //get all agencies under organisation
               
                var agencyhead = agencycode.Substring(0, 3);
                var agencyExist = await _context.Agencies.Where(x => x.AgencyCode.Equals(agencycode)).ToListAsync();
                if (agencyExist.Count == 0)
                {
                    var agencies = await _modelService.GetEBSAgencyByCode(agencyhead);
                    if (agencies.Count() != 0)
                    {
                        foreach (var item in agencies.DistinctBy(x => x.AgencyRef))
                        {

                            bool agencytype = false;
                            if (item.AgencyRef.Substring(3, 4) == "0000")
                            {
                                agencytype = true;
                            }
                            Agencies agenc = new Agencies
                            {
                                OrganisationId = organisation.OrganisationId,
                                AgencyCode = item.AgencyRef,
                                AgencyName = item.FullName,
                                Description = organisation.OrganisationName,
                                IsHead = agencytype,
                                Active = true,
                                DateCreated = DateTime.Now,
                                CreatedBy = "System",
                                TenantName = organisation.TenantName
                            };
                            _context.Agencies.Add(agenc);
                            _context.SaveChanges();
                            if (agencytype = true)
                            {
                                agencyId = agenc.AgencyId;
                            }

                        }
                        
                        //Add all revenue under agency head
                        var revenues = _modelService.GetEBSRevenueByCode(agencyhead);
                        //List<HeadRevenue> revlist = new();
                        foreach (var item in revenues.DistinctBy(x => x.RevCode))
                        {
                            /// var rev = revlist.Where(x => x.RevenueCode.Equals(item.RevCode)).SingleOrDefault();
                            // if (rev== null) {  
                            HeadRevenue revenue = new HeadRevenue
                            {
                                AgencyHead = agencyhead,
                                OrganisationId = id,
                                RevenueCode = item.RevCode,
                                RevenueName = item.RevName,
                                DateCreated = DateTime.Now,
                                CreatedBy = "System"
                            };
                            _context.HeadRevenue.Add(revenue);
                            _context.SaveChanges();
                            //revlist.Add(revenue);}
                        }
                    }
                    else
                    {
                        response.Status = 403;
                        response.StatusMessage = "No Sub-agency was found for " + agencycode + "update and try again";
                    }
                }

                //add organisation email to user table
                //map user
                var userexists = await _context.Users.Where(x => x.Email.Equals(organisation.Email ) && x.OrganisationId == organisation.OrganisationId).FirstOrDefaultAsync();
                int userId = 0;

                if (userexists != null) {
                    userId = userexists.UserId;
                } else { 
            UserCreationDto userCreationDto = new();
            userCreationDto.UserName = organisation.Email;
            userCreationDto.Email = organisation.Email;
            userCreationDto.PhoneNumber = organisation.PhoneNo;
            userCreationDto.AccountConfirmed = false;
            userCreationDto.LockoutEnabled = true;
            userCreationDto.DateCreated = DateTime.Now;
            userCreationDto.CreatedBy = organisation.Email;
            userCreationDto.Active = true;
            userCreationDto.TenantName = organisation.TenantName;
                    userCreationDto.AgencyId = agencyId;

            var userEntity = _mapper.Map<Users>(userCreationDto);
                //save to user table and customer table
                _repository.Users.CreateUserForOrganisation(organisation.OrganisationId, userEntity);

                //save to retrieve generated user id
                await _repository.SaveAsync();
                userId = userEntity.UserId;
            }
            var userRoleexists = await _context.UserRoles.Where(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
            var userRoleId = 0;
            if (userRoleexists != null)
            {
                userRoleId = userRoleexists.UserRoleId;
            }
            else
            {
                //map user role
                UserRoleCreationDto userRoleCreationDto = new();
                userRoleCreationDto.RoleId = 5;
                userRoleCreationDto.CreatedBy = organisation.Email;
                userRoleCreationDto.DateCreated = DateTime.Now;
                userRoleCreationDto.TenantName = organisation.TenantName;

                var userRoleEntity = _mapper.Map<UserRoles>(userRoleCreationDto);
                //save to user profile, user password and user role table
                _repository.UserRole.AddUserToRole(organisation.OrganisationId, userId, userRoleEntity);

                await _repository.SaveAsync();
                 userRoleId = userRoleEntity.UserRoleId;
            }




                //Generate password
                var pwd = new Password().IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial("[]{}^_=.!@£#$%").LengthRequired(8);
            var password = pwd.Next();

            //map user password
            UserPasswordCreationDto userPasswordCreationDto = new();
            userPasswordCreationDto.Password = password;
            userPasswordCreationDto.CreatedBy = organisation.Email;
            userPasswordCreationDto.TenantName = organisation.TenantName;
            userPasswordCreationDto.DateCreated = DateTime.Now;

            var userPasswordEntity = _mapper.Map<UserPasswords>(userPasswordCreationDto);

            //map password history table
            PasswordHistoryCreationDto passwordHistoryCreationDto = new();
            passwordHistoryCreationDto.Password = password;
            passwordHistoryCreationDto.DateCreated = DateTime.Now;
            passwordHistoryCreationDto.CreatedBy = organisation.Email;

            var passwordHistoryEntity = _mapper.Map<PasswordHistory>(passwordHistoryCreationDto);

            //store password in variable to send to user
            Password = userPasswordEntity.Password;

            //hash password 
            userPasswordEntity.Password = BCrypt.Net.BCrypt.HashPassword(userPasswordEntity.Password);
            passwordHistoryEntity.Password = userPasswordEntity.Password;

       

            

             _repository.UserPassword.AddUserPassword(organisation.OrganisationId, userId, userPasswordEntity);
            _repository.PasswordHistory.AddPassword(userId, passwordHistoryEntity);

            await _repository.SaveAsync();

            //generate otp for account activation
            var otpPwd = new Password(6).IncludeNumeric();
            var otp = otpPwd.Next();
           
            AccountActivation accountActivation = new AccountActivation
            {
                UserId = userId,
                RequestTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddDays(1), //set otp to expire after a day
                OTP = otp,
                DateCreated = DateTime.Now,
                CreatedBy = organisation.Email
            };
                //save otp to database
                await _context.AccountActivation.AddAsync(accountActivation);
                await _context.SaveChangesAsync();




                var userprofile = await _context.UserProfiles.Where(x => x.UserId.Equals(userId)).SingleOrDefaultAsync();
                if(userprofile == null) {
                
            //map user profile
            UserProfileCreationDto userProfileCreationDto = new();
            userProfileCreationDto.Firstname = organisation.Email;
            userProfileCreationDto.MiddleName = organisation.Email;
            userProfileCreationDto.Surname = organisation.Email;
            userProfileCreationDto.Email = organisation.Email;
            userProfileCreationDto.PhoneNumber = organisation.PhoneNo;
            userProfileCreationDto.DateCreated = DateTime.Now;
            userProfileCreationDto.CreatedBy = organisation.Email;
            userProfileCreationDto.Active = true;
            userProfileCreationDto.AgencyId = agencyId;
            userProfileCreationDto.UserRoleId = userRoleId;
            userProfileCreationDto.TenantName = organisation.TenantName;
            var userProfileEntity = _mapper.Map<UserProfiles>(userProfileCreationDto);
            _repository.UserProfile.CreateUserProfileForOrganisation(organisation.OrganisationId, userId, userRoleId, userProfileEntity, true);
            await _repository.SaveAsync();
                }

             var orgmodule= await _context.OrganisationModules.Where(x => x.OrganisationId == organisation.OrganisationId && x.ModuleId==7).FirstOrDefaultAsync();
                var rolemodulexist = await _context.RoleModules.Where(x => x.OrganisationId.Equals(organisation.OrganisationId) && x.RoleId.Equals(5)).SingleOrDefaultAsync()  ;

                if (orgmodule!=null && rolemodulexist==null)
                {
               
                            RoleModules roleModules = new RoleModules
                            {
                                ModuleId = orgmodule.ModuleId,
                                OrganisationId = orgmodule.OrganisationId,
                                RoleId = 5,
                                Active = true,
                                DateCreated = DateTime.Now,
                                CreatedBy = organisation.Email
                            };

                             _repository.RoleModules.CreateRoleModule(organisation.OrganisationId, roleModules);
                            
                    
                }
                //change status
                organisation.OrganisationApprovalStatus = (int)OrganisationApprovalStatusEnum.Approved;
                organisation.OrganisationStatus = true;
                organisation.AgencyCode = agencycode;
                _context.Update(organisation);
                _context.SaveChanges();

                //send success email message to organisation
                MailRequest mailRequest = new MailRequest();
                mailRequest.Subject = "Organisation RevBill Account Activation";
                mailRequest.ToEmail = organisation.Email;
                mailRequest.Password = Password;
                mailRequest.OTP = otp;
                mailRequest.OrganisationName = organisation.OrganisationName;
                await _mailService.SendApprovedOrganisationOnboardingRequest(mailRequest);
                response.Status = 403;
                response.StatusMessage = "Onboarding request successfully approved!";
            }
            return response;
        }

        public async Task RejectOnboardingRequestAsync(int id)
        {
            var organisation = await _context.Organisations.Where(x => x.OrganisationId.Equals(id)).FirstOrDefaultAsync();

            if (organisation is null)
                throw new IdNotFoundException("organisation", id);

            organisation.OrganisationApprovalStatus = (int)OrganisationApprovalStatusEnum.Rejected;

            //send rejection email message to organisation
        }

        public async Task<List<GetTenancyDto>> GetAllOrganisationTenantsAsync(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var summaryList = await _context.Tenancy
              .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
              .Take(validFilter.PageSize)
              .Where(x => x.Organisation.OrganisationApprovalStatus == 2)
              .Select(a => new
              {
                  Tenancy = a,
                  Organisation = _context.Organisations.Where(p => p.OrganisationId == a.OrganisationId).FirstOrDefault()
              }).ToListAsync();

            List<GetTenancyDto> result = new List<GetTenancyDto>();

            //add to list
            foreach (var item in summaryList)
            {
                result.Add(new GetTenancyDto
                {
                    Id = item.Tenancy.Id,
                    TenantId = item.Tenancy.TenantId,
                    ConnectionString = item.Tenancy.ConnectionString,
                    OrganisationName = item.Organisation.OrganisationName
                });
            }

            return result;
        }

        public async Task<List<GetTenancyDto>> GetOrganisationTenantAsync(int tenantId)
        {
            var summaryList = await _context.Tenancy
              .Where(x => x.Id == tenantId)
              .Select(a => new
              {
                  Tenancy = a,
                  Organisation = _context.Organisations.Where(p => p.OrganisationId == a.OrganisationId).FirstOrDefault()
              }).ToListAsync();

            List<GetTenancyDto> result = new List<GetTenancyDto>();

            //add to list
            foreach (var item in summaryList)
            {
                result.Add(new GetTenancyDto
                {
                    Id = item.Tenancy.Id,
                    TenantId = item.Tenancy.TenantId,
                    ConnectionString = item.Tenancy.ConnectionString,
                    OrganisationName = item.Organisation.OrganisationName
                });
            }

            return result;
        }

        public async Task<Response> UpdateOrganisationTenantAsync(int tenantId, UpdateTenancyDto updateTenant)
        {
            Response dataResponse = new Response();

            var tenant = await _context.Tenancy.Where(x => x.Id == tenantId).Select(x => x).FirstOrDefaultAsync();

            if (tenant is null)
            {
                dataResponse.StatusMessage = "Tenant not found";
                dataResponse.Status = 400;
            }

            else
            {
                var tenantEntity = _mapper.Map<UpdateTenancyDto>(updateTenant);

                //get updateTenant details
                var dataSource = tenantEntity.DataSource;
                var userId = tenantEntity.UserID;
                var password = tenantEntity.Password;
                var initialCatalog = tenantEntity.InitialCatalog;

                //concatenate to form connection string
                var connectionString = $"Data Source={dataSource};User ID={userId};Password={password};initial catalog={initialCatalog};integrated security=false;MultipleActiveResultSets=True;TrustServerCertificate=True";

                //save details
                tenant.ConnectionString = connectionString;
                tenant.ModifiedBy = tenantEntity.ModifiedBy;
                tenant.DateModified = DateTime.Now;

                await _context.SaveChangesAsync();

                dataResponse.StatusMessage = "Tenant successfully updated";
                dataResponse.Status = 200;
                dataResponse.Data = connectionString;
            }

            return dataResponse;
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", OrganisationId);
        }
    }
}