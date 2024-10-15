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
    //[Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class SampleController : ControllerBase
    {
        private readonly IServiceManager _service;

        public SampleController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all roles
        /// </summary>
        /// <returns>The roles list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetSample([FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.RoleService.GetAllRolesAsync(roleParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.roles);
        }
    }
}

