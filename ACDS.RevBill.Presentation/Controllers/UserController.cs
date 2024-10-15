using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Audit.WebApi;
using ACDS.RevBill.Shared.DataTransferObjects.Auth;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _service;

        public UserController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets all users in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}")]
        public async Task<IActionResult> GetUsersForOrganisation(int organisationId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.UserService.GetAllUsersInOrganisationAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.users);
        }

        /// <summary>
        /// Gets a single user in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/user/{id:int}", Name = "GetUserInOrganisation")]
        public async Task<IActionResult> GetUsersForOrganisation(int organisationId, int id)
        {
            var user = await _service.UserService.GetUserInOrganisationAsync(organisationId, id, trackChanges: false);
            return Ok(user);
        }

        /// <summary>
        /// creates a user for an organisation
        /// </summary>
        /// <response code="201">User Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/create-user")]
        public async Task<IActionResult> CreateUsersForOrganisation([AuditIgnore] int organisationId, [FromBody] UserRequestParams user)
        {
            if (user is null)
                return BadRequest("UserRequestParams object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var userToReturn = await _service.UserService.CreateUserForOrganisationAsync(organisationId, user, trackChanges: false);

            return Ok(userToReturn);
        }

        /// <summary>
        /// Updates a user using their id
        /// </summary>
        /// <response code="204">Successfully Updated User Profile</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/user/{id:int}")]
        public async Task<IActionResult> UpdateUserRole(int organisationId, int id, [FromBody] UserUpdateDto userUpdate)
        {
            if (userUpdate is null)
                return BadRequest("userUpdate object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.UserService.UpdateUserAsync(organisationId, id, userUpdate, trackOrgChanges: false, trackUserChanges: true);

            return Ok("User successfully updated");
        }

        /// <summary>
        /// Gets all user profiles in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/user-profile")]
        public async Task<IActionResult> GetUserProfilesForOrganisations(int organisationId, [FromQuery] PaginationFilter filter)
        {
            var userProfiles = await _service.UserService.GetAllUserProfilesAsync(organisationId, filter);

            return Ok(userProfiles);
        }
        /// <summary>
        /// Gets all user profiles in an organisation by agency
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/agency/{agencyId:int}/user-profile")]
        public async Task<IActionResult> GetAllUserProfilesbyAgency(int organisationId, int agencyId, [FromQuery] PaginationFilter filter)
        {
            var pagedResult = await _service.UserService.GetAllUserProfilesbyAgencyAsync(organisationId, agencyId, filter);
            return Ok(pagedResult);
        }
        /// <summary>
        /// Gets a single user profile in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/user-profile/{id:int}", Name = "GetUserProfileInOrganisation")]
        public async Task<IActionResult> GetUserProfileForOrganisation(int organisationId, int id)
        {
            var user = await _service.UserService.GetUserProfileAsync(organisationId, id);

            return Ok(user);
        }

        /// <summary>
        /// Updates the profile of a user using their id
        /// </summary>
        /// <response code="204">Successfully Updated User Profile</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/user-profile/{id:int}")]
        public async Task<IActionResult> UpdateUserRole(int organisationId, int id, [FromBody] UserProfileUpdateDto userProfileUpdate)
        {
            if (userProfileUpdate is null)
                return BadRequest("userProfileUpdate object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var update = await _service.UserService.UpdateUserProfileAsync(organisationId, id, userProfileUpdate,
            trackOrgChanges: false, trackUserProfileChanges: true);

            return Ok(update);
        }

        /// <summary>
        /// Gets all user roles in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/user-role")]
        public async Task<IActionResult>    GetAllUserRolesForOrganisation(int organisationId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.UserService.GetUserRolesAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.userRoles);
        }

      
        /// <summary>
        /// Gets a single user role in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="404">Error Not Found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{organisationId:int}/user-role/{id:int}", Name = "GetUserRoleInOrganisation")]
        public async Task<IActionResult> GetUserRoleForOrganisation(int organisationId, int id)
        {
            var user = await _service.UserService.GetUserRoleAsync(organisationId, id, trackChanges: false);
            return Ok(user);
        }

        /// <summary>
        /// Updates the role of a user using their id
        /// </summary>
        /// <response code="204">Successfully Updated User Role</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/user-role/{id:int}")]
        public async Task<IActionResult> UpdateUserRole(int organisationId, int id, [FromBody] UserRoleUpdateDto userRoleUpdate)
        {
            if (userRoleUpdate is null)
                return BadRequest("userRoleUpdate object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var update = await _service.UserService.UpdateUserRoleAsync(organisationId, id, userRoleUpdate,
            trackOrgChanges: false, trackUserRoleChanges: true);

            return Ok(update);
        }
    }
}