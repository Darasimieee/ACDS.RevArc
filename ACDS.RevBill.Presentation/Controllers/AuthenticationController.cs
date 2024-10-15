using ACDS.RevBill.Helpers;
using ACDS.RevBill.Repository;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Auth;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Presentation.Controllers
{
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [ApiController]
    [Route("api/auth")]
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly RepositoryContext _repositoryContext;
        private DataContext _context;

        public AuthenticationController(IServiceManager service, DataContext context, RepositoryContext repositoryContext)
        {
            _service = service;
            _context = context;
            _repositoryContext = repositoryContext;
        }

        /// <summary>
        /// Generates JWT token and logins in a user
        /// </summary>
        /// <response code="200">Token Generated</response>
        /// <response code="500">Internal Server Error</response> 
        /// <response code="401">Unauthorised</response> 
        /// <response code="400">Bad Request</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([AuditIgnore][FromBody] LoginRequest loginRequest)
        {
            if (loginRequest is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            //check if user exists in db
            var user = _context.Users.SingleOrDefault(u => (u.Email == loginRequest.Identifier || u.PhoneNumber == loginRequest.Identifier) && u.Active==true);
            if (user == null)
                return Unauthorized(new { StatusCode = 401, Message = "Invalid user or password" });

            var profile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.UserId);
            var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.UserId);

            //get password hash from user password table using user id
            var password = await _context.UserPasswords.Where(x => x.UserId == user.UserId).Select(y => y.Password).FirstOrDefaultAsync();

            //check if it's admin or superuser to force password change
            var userRole = await _context.UserRoles.Where(x => x.UserId == user.UserId).Select(y => y.RoleId).FirstOrDefaultAsync();
            var userPassword = await _context.PasswordHistory.Where(x => x.UserId == user.UserId).ToListAsync();
            var passwordCount = userPassword.Count();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, password))
                return Unauthorized(new { StatusCode = 401, Message = "Invalid user or password" });  

            if (user.AccountConfirmed == false)
                return Unauthorized(new { StatusCode = 401, Message = "Please kindly activate your account before you can login" });

            //if user is superadmin, admin or superuser, force password change
            if ((userRole == 1 || userRole == 2 || userRole == 3) && passwordCount == 1)
                return Unauthorized(new { StatusCode = 401, Message = "Please change your password" });

            // Pass the tenant or tenant ID to the RepositoryContext constructor
            var tenant = await _context.Tenancy.Where(x => x.OrganisationId == user.OrganisationId).FirstOrDefaultAsync();

            var tokenString = JwtHelper.GenerateJSONTenantToken(tenant);

            return Ok(new
            {
                Token = tokenString,
                UserId = user.UserId,
                RoleId = role.RoleId,
                Agency =profile.AgencyId,
                Agencytype=profile.IsHead,
                OrganisationId = user.OrganisationId,
                UserProfileId = profile.UserProfileId,
                AccountConfirmed = user.AccountConfirmed
            });
        }

        /// <summary>
        /// Customer Registration
        /// </summary>
        /// <response code="201">Customer successfully registered</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> CustomerRegistration([AuditIgnore][FromBody] RegisterRequest model)
        {
            if (model is null)
                return BadRequest("RegisterRequest object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var register = await _service.CustomerService.RegisterCustomerAsync(model, trackChanges: false);

            return Ok(register);
        }

        /// <summary>
        /// Request a password change
        /// </summary>
        /// <response code="200">successful</response>
        /// <response code="500">internal server error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(OTPRequest model)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var response=await _service.UserService.ForgotPasswordAsync(model);
            return Ok(response);
        }

        /// <summary>
        /// Validate an OTP for forgot password request
        /// </summary>
        /// <response code="200">successful</response>
        /// <response code="500">internal server error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("verify-password-otp")]
        public async Task<IActionResult> VerifyForgotPasswordOTP(ValidateOTPRequest model)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var password = await _service.UserService.VerifyForgotPasswordOTPAsync(model);
            return Ok(password);
        }

        /// <summary>
        /// Update a password
        /// </summary>
        /// <response code="200">successful</response>
        /// <response code="500">internal server error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordParams user)
        {
            if (user is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var password = await _service.UserService.UpdatePasswordAsync(user, trackOrgChanges: false, trackUserPasswordChanges: true);
            return Ok(password);
        }

        /// <summary>
        /// Confirms a newly registered account
        /// </summary>
        /// <response code="200">successful</response>
        /// <response code="500">internal server error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("confirm-account")]
        public async Task<IActionResult> ConfirmAccount(ValidateOTPRequest model)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var account = await _service.UserService.ConfirmAccount(model);
            return Ok(account);
        }

        /// <summary>
        /// Generate OTP for newly registered account
        /// </summary>
        /// <response code="200">successful</response>
        /// <response code="500">internal server error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("confirm-account-otp")]
        public async Task<IActionResult> ConfirmAccountOTP(OTPRequest model)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.UserService.GenerateConfirmAccountOTP(model);
            return Ok(new { message = "An OTP has been sent to your registered email and phone number." });
        }
    }
}