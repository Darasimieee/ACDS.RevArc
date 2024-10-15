using System;
using System.ComponentModel.Design;
using System.Text.Json;
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
    [Route("api/roles")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class RolesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RolesController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all roles
        /// </summary>
        /// <returns>The roles list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.RoleService.GetAllRolesAsync(roleParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.roles);
        }

        /// <summary>
        /// Gets a specific role using their id
        /// </summary>
        /// <returns>The role</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{Id:int}", Name = "RoleById")]
        public async Task<IActionResult> GetRole(int Id)
        {
            var role = await _service.RoleService.GetRoleAsync(Id, trackChanges: false);

            return Ok(role);
        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <returns>Successful role creation</returns>
        /// <response code="201">Role Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleForCreationDto role)
        {
            if (role is null)
                return BadRequest("RoleForCreationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var createdRole = await _service.RoleService.CreateRoleAsync(role);

            return CreatedAtRoute("RoleById", new { id = createdRole.RoleId }, createdRole);
        }

        /// <summary>
        /// Updates a specific role using their id
        /// </summary>
        /// <response code="204">Successfully Updated Role</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateRole(int Id, [FromBody] RoleForUpdateDto role)
        {
            if (role is null)
                return BadRequest("RoleForUpdateDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.RoleService.UpdateRoleAsync(Id, role, trackChanges: true);

            return NoContent();
        }
    }
}

