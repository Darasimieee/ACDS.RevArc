using System.Text.Json;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/organisation")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class OrganisationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public OrganisationController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all Organisations
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetOrganisations([FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.OrganisationService.GetAllOrganisationsAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.organisations);
        }

        /// <summary>
        /// Gets the list of all organisations without images
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("list")]
        public async Task<IActionResult> GetOrganisationsWithoutImages([FromQuery] PaginationFilter filter)
        {
            var result = await _service.OrganisationService.GetAllOrganisationsWithoutImagesAsync(filter);

            return Ok(result);
        }

        /// <summary>
        /// Gets a specific Organisation using their id
        /// </summary>
        /// <returns>The Organisation</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}", Name = "OrganisationById")]
        public async Task<IActionResult> GetOrganisation(int organisationId)
        {
            var organisation = await _service.OrganisationService.GetOrganisationAsync(organisationId, trackChanges: false);

            return Ok(organisation);
        }

        /// <summary>
        /// Creates a new Organisation
        /// </summary>
        /// <returns>Organisation Successfully Created</returns>
        /// <response code="201">Organisation Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateOrganisation([FromForm] CreateOrganisationDto organisation)
        {
            if (organisation is null)
                return BadRequest("CreateOrganisationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var createdOrganisation = await _service.OrganisationService.CreateOrganisationAsync(organisation);

            return Ok(createdOrganisation);
        }

        /// <summary>
        /// Updates a specific Organisation using their id
        /// </summary>
        /// <response code="204">Successfully Updated Organisation</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}")]
        public async Task<IActionResult> UpdateOrganisation(int organisationId, [FromForm] UpdateOrganisationDto organisation)
        {
            if (organisation is null)
                return BadRequest("UpdateOrganisationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var org = await _service.OrganisationService.UpdateOrganisationAsync(organisationId, organisation, trackChanges: true);

            return Ok(org);
        }

        /// <summary>
        /// Gets the list of all approved onboarding requests
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("approved")]
        public async Task<IActionResult> ApprovedOnboardingRequests()
        {
            var organisations = await _service.OrganisationService.ApprovedOnboardingRequestsAsync();

            return Ok(organisations);
        }

        /// <summary>
        /// Gets the list of all unapproved onboarding requests
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("unapproved")]
        public async Task<IActionResult> RejectedOnboardingRequests()
        {
            var organisations = await _service.OrganisationService.RejectedOnboardingRequestsAsync();

            return Ok(organisations);
        }

        /// <summary>
        /// Gets the list of all pending onboarding requests
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("pending")]
        public async Task<IActionResult> PendingOnboardingRequests()
        {
            var organisations = await _service.OrganisationService.PendingOnboardingRequestsAsync();
             
            return Ok(organisations);
        }

        /// <summary>
        /// Approve onboarding requests from an organisation
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("approve-onboarding-request/{organisationId:int}/agency/{agencycode}")]
        public async Task<IActionResult> ApproveOnboardingRequest(int organisationId ,string agencycode)
        {
           var response= await _service.OrganisationService.ApproveOnboardingRequestAsync(organisationId, agencycode);

            return Ok(response);
        }

        /// <summary>
        /// Reject onboarding requests from an organisation
        /// </summary>
        /// <returns>The Organisations list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("reject-onboarding-request/{organisationId:int}")]
        public async Task<IActionResult> RejectOnboardingRequest(int organisationId)
        {
            await _service.OrganisationService.RejectOnboardingRequestAsync(organisationId);

            return Ok(new
            {
                message = ("Onboarding request successfully rejected")
            });
        }

        /// <summary>
        /// Deactivates an organisation
        /// </summary>
        /// <returns>Organisation successfully deactivated</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/deactivate")]
        public async Task<IActionResult> DeactivateOrganisation(int organisationId)
        {
            await _service.OrganisationService.DeactivateOrganisation(organisationId);

            return Ok(new
            {
                message = ("Organisation successfully deactivated")
            });
        }

        /// <summary>
        /// Activates an organisation
        /// </summary>
        /// <returns>Organisation successfully activated</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/activate")]
        public async Task<IActionResult> ActivateOrganisation(int organisationId)
        {
            await _service.OrganisationService.ActivateOrganisation(organisationId);

            return Ok(new
            {
                message = ("Organisation successfully activated")
            });
        }

        /// <summary>
        /// Gets the list of all Organisation Tenants
        /// </summary>
        /// <returns>The Organisations tenants list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("tenants")]
        public async Task<IActionResult> GetOrganisationTenants([FromQuery] PaginationFilter filter)
        {
            var result = await _service.OrganisationService.GetAllOrganisationTenantsAsync(filter);

            return Ok(result);
        }

        /// <summary>
        /// Gets a specific Organisation Tenant using their id
        /// </summary>
        /// <returns>The Organisation</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("tenants/{tenantId:int}")]
        public async Task<IActionResult> GetOrganisationTenant(int tenantId)
        {
            var organisation = await _service.OrganisationService.GetOrganisationTenantAsync(tenantId);

            return Ok(organisation);
        }

        /// <summary>
        /// Updates an organisation tenant
        /// </summary>
        /// <returns>Updated organisation tenant</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("tenants/update/{tenantId:int}")]
        public async Task<IActionResult> UpdateOrganisationTenant(int tenantId, [FromBody] UpdateTenancyDto updateTenant)
        {
            if (updateTenant is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var tenant =  await _service.OrganisationService.UpdateOrganisationTenantAsync(tenantId, updateTenant);

            return Ok(tenant);
        }
    }
}