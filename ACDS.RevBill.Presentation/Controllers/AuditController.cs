using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/audit")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class AuditController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuditController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the audit logs.
        /// </summary>
        /// <response code="200">successful</response>
        /// <response code="401">unauthorised</response>
        /// <response code="500">internal server error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("logs")]
        public async Task<IActionResult> GetAllLogs([FromQuery] PaginationFilter filter)
        {
            var response = await _service.AuditTrailService.GetAuditTrailAsync(filter);

            return Ok(response);
        }
    }
}