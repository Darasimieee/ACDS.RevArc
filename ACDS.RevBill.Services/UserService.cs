using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Email;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Auth;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;

namespace ACDS.RevBill.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private IMailService _mailService;
        private DataContext _context;
        private bool ComparePassword { get; set; }
        private string? Password { get; set; }
        SmsUtility SmsUtility = new SmsUtility();

        public UserService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IMailService mailService,
            DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _mailService = mailService;
            _context = context;
        }

        public async Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersInOrganisationAsync(int organisationId, DefaultParameters requestParameters,
            bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var usersWithMetaData = await _repository.Users.GetAllUsersAsync(organisationId, requestParameters, trackChanges);

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersWithMetaData);

            return (users: usersDto, metaData: usersWithMetaData.MetaData);
        }

        public async Task<Response> CreateUserForOrganisationAsync(int organisationId, UserRequestParams user, bool trackChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackChanges);

            //check to see if email and username already exists
            var userexist =await CheckIfEmailExists(organisationId, user.Email, trackChanges);
            if (userexist != null)
            {
                dataResponse.Status = 200;
                dataResponse.StatusMessage = "User already exist!";
                
            }
            var organisation = await _context.Organisations
                                .Where(x => x.OrganisationId == organisationId)
                                .Select(x => new { x.Email, x.TenantName })
                                .ToListAsync();
            
            var organisationEmail = organisation.FirstOrDefault()?.Email;
            var organisationTenantId = organisation.FirstOrDefault()?.TenantName;

            //map user
            UserCreationDto userCreationDto = new();
            userCreationDto.UserName = user.Email;
            userCreationDto.Email = user.Email;
            userCreationDto.PhoneNumber = user.PhoneNumber;
            userCreationDto.AccountConfirmed = false;
            userCreationDto.LockoutEnabled = true;
            userCreationDto.DateCreated = user.DateCreated;
            userCreationDto.CreatedBy = user.CreatedBy;
            userCreationDto.Active = true;
            userCreationDto.AgencyId = user.AgencyId;
            userCreationDto.TenantName = organisationTenantId;
            

            var userEntity = _mapper.Map<Users>(userCreationDto);
            userEntity.AccessFailedCount = 0;
            userEntity.OrganisationId = organisationId;
            

            //map user role
            UserRoleCreationDto userRoleCreationDto = new();
            userRoleCreationDto.RoleId = user.RoleId;
            userRoleCreationDto.CreatedBy = user.CreatedBy;
            userRoleCreationDto.DateCreated = user.DateCreated;
            userRoleCreationDto.TenantName = organisationTenantId;

            var userRoleEntity = _mapper.Map<UserRoles>(userRoleCreationDto);
           
            //map user profile
            UserProfileCreationDto userProfileCreationDto = new();
            userProfileCreationDto.Firstname = user.Firstname;
            userProfileCreationDto.MiddleName = user.MiddleName;
            userProfileCreationDto.Surname = user.Surname;
            userProfileCreationDto.Email = user.Email;
            userProfileCreationDto.PhoneNumber = user.PhoneNumber;
            userProfileCreationDto.DateCreated = user.DateCreated;
            userProfileCreationDto.CreatedBy = user.CreatedBy;
            userProfileCreationDto.Active = true;
            userProfileCreationDto.AgencyId= user.AgencyId;
            userProfileCreationDto.UserRoleId = userRoleEntity.UserRoleId;
            userProfileCreationDto.TenantName = organisationTenantId;
            

            var userProfileEntity = _mapper.Map<UserProfiles>(userProfileCreationDto);

            //Generate password
            var pwd = new Password().IncludeLowercase().IncludeUppercase().IncludeNumeric().IncludeSpecial("[]{}^_=.!@£#$%").LengthRequired(8);
            var password = pwd.Next();

            //map user password
            UserPasswordCreationDto userPasswordCreationDto = new();
            userPasswordCreationDto.Password = password;
            userPasswordCreationDto.CreatedBy = user.CreatedBy;
            userPasswordCreationDto.DateCreated = user.DateCreated;

            var userPasswordEntity = _mapper.Map<UserPasswords>(userPasswordCreationDto);

            //map password history table
            PasswordHistoryCreationDto passwordHistoryCreationDto = new();
            passwordHistoryCreationDto.Password = password;
            passwordHistoryCreationDto.DateCreated = user.DateCreated;
            passwordHistoryCreationDto.CreatedBy = user.CreatedBy;

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
            var agency= await _context.Agencies.Where(x => x.AgencyId.Equals(user.AgencyId)).SingleAsync();
        
            _repository.UserProfile.CreateUserProfileForOrganisation(organisationId, userEntity.UserId, userRoleEntity.UserRoleId, userProfileEntity, agency.IsHead);
            _repository.UserPassword.AddUserPassword(organisationId, userEntity.UserId, userPasswordEntity);
            _repository.PasswordHistory.AddPassword(userEntity.UserId, passwordHistoryEntity);
            _logger.LogInfo("I got here" + userEntity);
            await _repository.SaveAsync();

            //generate otp for account activation
            var otpPwd = new Password(6).IncludeNumeric();
            var otp = otpPwd.Next();

            AccountActivation accountActivation = new AccountActivation
            {
                UserId = userEntity.UserId,
                RequestTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(60), //set otp to expire after an hour
                OTP = otp,
                DateCreated = DateTime.Now,
                CreatedBy = user.Email
            };
            _logger.LogInfo("I got here" + accountActivation);
            //save otp to database
            await _context.AccountActivation.AddAsync(accountActivation);
            await _context.SaveChangesAsync();

            //send activation email to user
            MailRequest mailRequest = new MailRequest();
            mailRequest.Subject = "RevBill Account Activation";
            mailRequest.ToEmail = user.Email;
            mailRequest.Password = Password;
            mailRequest.OTP = otp;
            mailRequest.FirstName = userProfileEntity.Firstname;
            mailRequest.LastName = userProfileEntity.Surname;

            await _mailService.SendWelcomeEmailAsync(mailRequest);

            //response message
            dataResponse.StatusMessage = "User successfully created";
            dataResponse.Status = 200;
            dataResponse.Data = userProfileEntity;

            return dataResponse;
        }

        public async Task<UserDto> GetUserInOrganisationAsync(int organisationId, int userId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var userDb = await CheckIfUserExists(organisationId, userId, trackChanges);

            var user = _mapper.Map<UserDto>(userDb);

            return user;
        }

        public async Task UpdateUserAsync(int organisationId, int userId, UserUpdateDto userUpdate,
           bool trackOrgChanges, bool trackUserChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackOrgChanges);

            var userDb = await CheckIfUserExists(organisationId, userId, trackUserChanges);

            _mapper.Map(userUpdate, userDb);
            await _repository.SaveAsync();
        }

        public async Task<Response> ForgotPasswordAsync(OTPRequest model)
        {
            Response dataResponse= new Response();
            var getUser = await _context.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefaultAsync();
            if (getUser is null)
            {
                dataResponse.StatusMessage = "User not found";
                dataResponse.Status = 404;
                return dataResponse;
            }
            //generate otp
            var pwd = new Password(6).IncludeNumeric();
            var otp = pwd.Next();

            UserPasswordResetRequests userPasswordReset = new UserPasswordResetRequests
            {
                UserId = getUser.UserId,
                RequestTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(10), //set otp to expire after ten minutes
                OTP = otp,
                DateCreated = DateTime.Now,
                CreatedBy = getUser.Email
            }; //save otp to database

            await _context.UserPasswordResetRequests.AddAsync(userPasswordReset);
            await _context.SaveChangesAsync();

            //send otp email to user
            MailRequest mailRequest = new MailRequest();
            mailRequest.Subject = "Forgot Password";
            mailRequest.ToEmail = model.Email;
            mailRequest.OTP = otp;

            //send sms for otp
            MessageQueue messageQueue = new MessageQueue();
            messageQueue.Phone = getUser.PhoneNumber;
            messageQueue.Message = $"Reset your Revbill account - {model.Email} password using this OTP - {otp}. It is valid for ten minutes";

            //send otp to sms
            SmsUtility.SendSMS(messageQueue);

            await _mailService.SendForgotPasswordEmailAsync(mailRequest);
            dataResponse.StatusMessage = "An OTP has been sent to your registered email and phone number.";
            dataResponse.Status = 200;
            return dataResponse;

        }

        public async Task<Response> VerifyForgotPasswordOTPAsync(ValidateOTPRequest model)
        {
            Response dataResponse = new Response();

            var getUser = await _context.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefaultAsync();

            var resetRequests = await _context.UserPasswordResetRequests.Where(x => x.UserId == getUser.UserId)
                .OrderBy(x => x.UserPasswordResetRequestId)
                .LastOrDefaultAsync();

            //check if otp is valid
            if (resetRequests.OTP != model.OTP)
            {
                dataResponse.StatusMessage = "Invalid OTP, Does not match";
                dataResponse.Status = 400;
            }

            //check if otp is expired
            if (DateTime.Now > resetRequests.ExpireTime)
            {
                dataResponse.StatusMessage = "Invalid OTP, OTP expired";
                dataResponse.Status = 400;
            }

            else if (resetRequests.OTP == model.OTP)
            {
                dataResponse.StatusMessage = "OTP verified successfully. You can now change your password";
                dataResponse.Status = 200;
            }

            return dataResponse;
        }

        public async Task<Response> UpdatePasswordAsync(UpdatePasswordParams user, bool trackOrgChanges, bool trackUserPasswordChanges)
        {
            Response dataResponse = new Response();

            var getUser = await _context.Users.Where(x => x.Email.Equals(user.UserPasswordUpdateDto.Email)).FirstOrDefaultAsync();
            if (getUser.AccountConfirmed == false)
            {
                getUser.AccountConfirmed = true;    
                 _context.Users.Update(getUser);
                _context.SaveChanges();
            }
            var getUserPassword = await _context.UserPasswords.Where(x => x.UserId.Equals(getUser.UserId)).FirstOrDefaultAsync();
            var getPasswordHistory = await _context.PasswordHistory.Where(x => x.UserId.Equals(getUser.UserId)).FirstOrDefaultAsync();

            //get last six passwords
            var lastSixPasswords = _context.PasswordHistory
                .Where(x => x.UserId == getUser.UserId)
                .OrderByDescending(p => p.PasswordHistoryId)
                .Take(6);

            List<string> myPassword = new List<string>();

            foreach (var password in lastSixPasswords)
            {
                //add passwords to list
                myPassword.Add(password.Password);
            }

            bool isPasswordUsed = false; // Flag to track if the password is already used

            //compare new password with password in password history table to ensure it hasn't been used
            for (int i = 0; i < myPassword.Count(); i++)
            {
                ComparePassword = BCrypt.Net.BCrypt.Verify(user.UserPasswordUpdateDto.Password, myPassword[i]);
                if (ComparePassword == true)
                {
                    isPasswordUsed = true; // Set the flag to true if password is already used
                    break; // Exit the loop since the condition is met
                }
            }

            if (isPasswordUsed)
            {
                dataResponse.StatusMessage = "You have used this password already. Enter a different password";
                dataResponse.Status = 400;
            }
            else
            {
                //hash new password
                var newPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPasswordUpdateDto.Password);

                //map user password table
                var userPasswordDb = await _repository.UserPassword.GetUserPasswordAsync((int)getUserPassword.UserPasswordId, trackUserPasswordChanges);
                _mapper.Map(user.UserPasswordUpdateDto, userPasswordDb);
                userPasswordDb.Password = newPassword;

                //add new password to password history table
                //map password history table
                var passwordHistoryEntity = _mapper.Map<PasswordHistory>(user.PasswordHistoryCreationDto);
                passwordHistoryEntity.Password = newPassword;

                _repository.PasswordHistory.AddPassword(getUser.UserId, passwordHistoryEntity);
                await _repository.SaveAsync();

                //send otp email to user
                MailRequest mailRequest = new MailRequest();
                mailRequest.Subject = "Password Update";
                mailRequest.ToEmail = getUser.Email;

                await _mailService.SendPasswordUpdateEmailAsync(mailRequest);

                dataResponse.StatusMessage = "Password changed successfully";
                dataResponse.Status = 200;
            }

            return dataResponse;
        }

        public async Task<Response> ConfirmAccount(ValidateOTPRequest model)
        {
            Response dataResponse = new Response();

            var getUser = await _context.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefaultAsync();

            var activation = await _context.AccountActivation.Where(x => x.UserId == getUser.UserId)
                .OrderBy(x => x.AccountActivationId)
                .LastOrDefaultAsync();

            //check if otp is valid
            if (activation.OTP != model.OTP)
            {
                dataResponse.StatusMessage = "Invalid OTP, Does not match";
                dataResponse.Status = 400;
            }

            //check if otp is expired
            if (DateTime.Now > activation.ExpireTime)
            {
                dataResponse.StatusMessage = "Invalid OTP, OTP expired";
                dataResponse.Status = 400;
            }

            else if (activation.OTP == model.OTP)
            {
                //set account confirmed to true
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);
                user.AccountConfirmed = true;

                await _context.SaveChangesAsync();

                dataResponse.StatusMessage = "Account Confirmed. You can now login";
                dataResponse.Status = 200;
            }                       

            return dataResponse;
        }

        public async Task GenerateConfirmAccountOTP(OTPRequest model)
        {
            var user = await _context.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefaultAsync();

            //generate otp for account activation
            var pwd = new Password(6).IncludeNumeric();
            var otp = pwd.Next();

            AccountActivation accountActivation = new AccountActivation
            {
                UserId = user.UserId,
                RequestTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(10), //set otp to expire after ten minutes
                OTP = otp,
                DateCreated = DateTime.Now,
                CreatedBy = model.Email
            };

            //save otp to database
            await _context.AccountActivation.AddAsync(accountActivation);
            await _context.SaveChangesAsync();

            //send activation email to user
            MailRequest mailRequest = new MailRequest();
            mailRequest.Subject = "RevBill Account Activation";
            mailRequest.ToEmail = model.Email;
            mailRequest.OTP = otp;

            //send sms for otp
            MessageQueue messageQueue = new MessageQueue();
            messageQueue.Phone = user.PhoneNumber;
            messageQueue.Message = $"Kindly confirm your Revbill account - {model.Email} using this OTP - {otp}. It is valid for ten minutes";

            //send otp to sms
            SmsUtility.SendSMS(messageQueue);

            await _mailService.GenerateAccountActivationEmailAsync(mailRequest);
        }

        public async Task<List<UserProfileDto>> GetAllUserProfilesAsync(int organisationId, PaginationFilter filter)
        {
            var profileList = await _context.UserProfiles
                .Where(x => x.OrganisationId == organisationId)
                .Select(a => new
                {
                    Id = a.UserProfileId,
                    Users = _context.Users.Where(c => c.UserId == a.UserId).FirstOrDefault(),
                    UserProfiles = a,
                    Roles = _context.Roles.Where(c => c.RoleId == a.UserRoles.RoleId).FirstOrDefault(),
                    Agency=_context.Agencies.Where(c => c.AgencyId==a.AgencyId).FirstOrDefault(),
                    Organisation = _context.Organisations.Where(p => p.OrganisationId == organisationId).FirstOrDefault()
                }).ToListAsync();

            List<UserProfileDto> result = new List<UserProfileDto>();
            if (profileList.Count > 0) {
                foreach (var item in profileList)
            {
                    var agencyname = "";
                    if (item.Agency.AgencyName != null) {
                         agencyname = item.Agency.AgencyName;
                    };
                    var resultitem = new UserProfileDto
                    {
                        UserProfileId = item.Id,
                        Surname = item.UserProfiles.Surname,
                        Firstname = item.UserProfiles.Firstname,
                        Middlename = item.UserProfiles.Middlename,
                        Email = item.UserProfiles.Email,
                        PhoneNumber = item.UserProfiles.PhoneNumber,
                        Active = item.Users.Active,
                        Confirmed = item.Users.AccountConfirmed,
                        DateCreated = (DateTime)item.Users.DateCreated,
                        RoleName = item.Roles.RoleName,
                        AgencyName = agencyname,
                        Organisation = item.Organisation
                    };
                result.Add(resultitem);
                }
            }
            return result;
        }
        public async Task<List<UserProfileDto>> GetAllUserProfilesbyAgencyAsync(int organisationId,int agencyId, PaginationFilter filter)
        {
            var profileList = await _context.UserProfiles
                .Where(x => x.OrganisationId == organisationId && x.AgencyId==agencyId)
                .Select(a => new
                {
                    Id = a.UserProfileId,
                    Users = _context.Users.Where(c => c.UserId == a.UserId).FirstOrDefault(),
                    UserProfiles = a,
                    Roles = _context.Roles.Where(c => c.RoleId == a.UserRoles.RoleId).FirstOrDefault(),
                    Agency = _context.Agencies.Where(c => c.AgencyId == a.AgencyId).FirstOrDefault(),
                    Organisation = _context.Organisations.Where(p => p.OrganisationId == organisationId).FirstOrDefault()
                }).ToListAsync();

            List<UserProfileDto> result = new List<UserProfileDto>();
            if (profileList.Count > 0)
            {
                foreach (var item in profileList)
                {
                    result.Add(new UserProfileDto
                    {
                        UserProfileId = item.Id,
                        Surname = item.UserProfiles.Surname,
                        Firstname = item.UserProfiles.Firstname,
                        Middlename = item.UserProfiles.Middlename,
                        Email = item.UserProfiles.Email,
                        PhoneNumber = item.UserProfiles.PhoneNumber,
                        Active = item.Users.Active,
                        Confirmed = item.Users.AccountConfirmed,
                        DateCreated = (DateTime)item.Users.DateCreated,
                        RoleName = item.Roles.RoleName,
                        AgencyName = item.Agency.AgencyName,
                        Organisation = item.Organisation
                    });
                }
            }
            return result;
        }
        public async Task<List<UserProfileDto>> GetUserProfileAsync(int organisationId, int userId)
        {
            //get user profile id
            var userProfileId = await _context.UserProfiles
                .Where(x => x.OrganisationId == organisationId && x.UserId == userId)
                .Select(x => x.UserProfileId).FirstOrDefaultAsync();

            var profileList = await _context.UserProfiles
                .Where(x => x.OrganisationId == organisationId && x.UserProfileId == userProfileId)
                .Select(a => new
                {
                    Id = a.UserProfileId,
                    Users = _context.Users.Where(c => c.UserId == a.UserId).FirstOrDefault(),
                    UserProfiles = a,
                    Roles = _context.Roles.Where(c => c.RoleId == a.UserRoles.RoleId).FirstOrDefault(),
                    Organisation = _context.Organisations.Where(p => p.OrganisationId == organisationId).FirstOrDefault()
                }).ToListAsync();

            List<UserProfileDto> result = new List<UserProfileDto>();
            
            foreach (var item in profileList)
            {
                result.Add(new UserProfileDto
                {
                    UserProfileId = item.Id,
                    RoleName = item.Roles.RoleName,
                    Organisation = item.Organisation,
                    Surname = item.UserProfiles.Surname,
                    Firstname = item.UserProfiles.Firstname,
                    Middlename = item.UserProfiles.Middlename,
                    Email = item.UserProfiles.Email,
                    PhoneNumber = item.UserProfiles.PhoneNumber,
                    Active = item.Users.Active,
                    Confirmed = item.Users.AccountConfirmed,
                    DateCreated = (DateTime)item.Users.DateCreated
                });
            }
            return result;
        }

        public async Task<Response> UpdateUserProfileAsync(int organisationId, int userId, UserProfileUpdateDto userProfileUpdate,
          bool trackOrgChanges, bool trackUserProfileChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackOrgChanges);

            //get user profile id
            var userProfileId = await _context.UserProfiles
                .Where(x => x.OrganisationId == organisationId && x.UserId == userId)
                .Select(x => x.UserProfileId).FirstOrDefaultAsync();

            var userProfileDb = await CheckIfUserProfileExists(organisationId, userProfileId, trackUserProfileChanges);

            _mapper.Map(userProfileUpdate, userProfileDb);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Profile succesfully updated";
            dataResponse.Status = 200;

            return dataResponse;
        }

        public async Task<UserRoleDto> GetUserRoleAsync(int organisationId, int userId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            //get user role id
            var userRoleId = await _context.UserRoles
                .Where(x => x.OrganisationId == organisationId && x.UserId == userId)
                .Select(x => x.UserRoleId).FirstOrDefaultAsync();

            var userRoleDb = await CheckIfUserRoleExists(organisationId, userRoleId, trackChanges);

            var userRole = _mapper.Map<UserRoleDto>(userRoleDb);

            return userRole;
        }

        public async Task<(IEnumerable<UserRoleDto> userRoles, MetaData metaData)> GetUserRolesAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var userRolesWithMetaData = await _repository.UserRole.GetAllUserRolesAsync(organisationId, requestParameters, trackChanges);

            var userRolesDto = _mapper.Map<IEnumerable<UserRoleDto>>(userRolesWithMetaData);

            return (userRoles: userRolesDto, metaData: userRolesWithMetaData.MetaData);
        }

        public async Task<Response> UpdateUserRoleAsync(int organisationId, int userId, UserRoleUpdateDto userRoleUpdate,
            bool trackOrgChanges, bool trackUserRoleChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackOrgChanges);

            //get user role id
            var userRoleId = await _context.UserRoles
                .Where(x => x.OrganisationId == organisationId && x.UserId == userId)
                .Select(x => x.UserRoleId).FirstOrDefaultAsync();

            await CheckIfRoleExists(userRoleUpdate.RoleId, trackUserRoleChanges);
            var userRoleDb = await CheckIfUserRoleExists(organisationId, userRoleId, trackUserRoleChanges);

            _mapper.Map(userRoleUpdate, userRoleDb);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Role succesfully updated";
            dataResponse.Status = 200;
            dataResponse.Data = userRoleDb.RoleId;

            return dataResponse;
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int organisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(organisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", organisationId);
        }

        private async Task<Users> CheckIfUserExists(int organisationId, int userId, bool trackChanges)
        {
            var userDb = await _repository.Users.GetUserAsync(organisationId, userId, trackChanges);
            if (userDb is null)
                throw new IdNotFoundException("user", userId);

            return userDb;
        }

        private async Task<Users> CheckIfEmailExists(int organisationId, string email, bool trackChanges)
        {
            var userDb = await _repository.Users.GetEmailAsync(organisationId, email, trackChanges);
            if (userDb is not null)
                return null;

            return userDb;
        }

        private async Task<UserProfiles> CheckIfUserProfileExists(int organisationId, int userProfileId, bool trackChanges)
        {
            var userProfileDb = await _repository.UserProfile.GetUserProfileAsync(organisationId, userProfileId, trackChanges);
            if (userProfileDb is null)
                throw new IdNotFoundException("user profile", userProfileId);

            return userProfileDb;
        }

        private async Task<UserRoles> CheckIfUserRoleExists(int organisationId, int userRoleId, bool trackChanges)
        {
            var userRoleDb = await _repository.UserRole.GetUserRoleAsync(organisationId, userRoleId, trackChanges);
            if (userRoleDb is null)
                throw new IdNotFoundException("user role", userRoleId);

            return userRoleDb;
        }

        private async Task<Roles> CheckIfRoleExists(int roleId, bool trackChanges)
        {
            var roleDb = await _repository.Roles.GetRoleAsync(roleId, trackChanges);
            if (roleDb is null)
                throw new IdNotFoundException("role", roleId);

            return roleDb;
        }
    }
}