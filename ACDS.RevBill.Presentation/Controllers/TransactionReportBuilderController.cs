using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency;
using Audit.WebApi;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/transactionReport")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class TransactionReportBuilderController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        public TransactionReportBuilderController(IServiceManager service, IMapper mapper) { _service = service; _mapper = mapper; }

        /// <summary>
        /// Total Property Count
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/total-property-count")]
        public async Task<IActionResult> TotalPropertyCount(int organisationId)
        {
            var count = await _service.TransactionReportBuilderService.TotalPropertyCount(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Total Property occupied by customer Count
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/total-customer-property-count")]
        public async Task<IActionResult> TotalCustomerPropertyCount(int organisationId)
        {
            var count = await _service.TransactionReportBuilderService.TotalCustomerInPropertyCount(organisationId);

            return Ok(count);
        }
        /// <summary>
        /// Total Non Property Count
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/total-non-property-count")]
        public async Task<IActionResult> TotalNonPropertyCount(int organisationId)
        {
            var count = await _service.TransactionReportBuilderService.TotalNonPropertyCount(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Total Customer Count
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/total-customer-count")]
        public async Task<IActionResult> TotalCustomerCount(int organisationId)
        {
            var count = await _service.TransactionReportBuilderService.TotalCustomerCount(organisationId);

            return Ok(count);
        }
        /// <summary>
        /// Agency Yearly Collection
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("agency_yearly_collection")]
        public async Task<IActionResult> AgencyYearlyCollection([FromBody] AgencyYearlyCollectionRequest agency)
        {
            
            var result = await _service.TransactionReportBuilderService.AgencyYearlyCollection(agency);

            return Ok(result);
        }
        /// <summary>
        /// Agency Quarterly Collection
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("agency_quarterly_collection")]
        public async Task<IActionResult> AgencyQuarterlyCollection([FromBody] AgencyQuarterlyCollectionRequest agencyQuarter)
        {

            var result = await _service.TransactionReportBuilderService.AgencyQuarterlyCollection(agencyQuarter);
           
            return Ok(result);
        }
        /// <summary>
        /// Agency Quarterly Collection
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("agency_biannual_collection")]
        public async Task<IActionResult> AgencyBiAnnualCollection([FromBody] AgencyBiAnnualCollectionRequest agencyBianual)
        {

            var result = await _service.TransactionReportBuilderService.GetAgencyBiAnnualCollection(agencyBianual);
      
            return Ok(result);
        }
        /// <summary>
        /// Gets Payments using some parameters
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("debt-filter-report/{organisationId:int}")]
        public async Task<IActionResult> FilterDebts(int organisationId, [FromQuery] DebtReportParameters requestParameters)
        {
            var pagedResult = await _service.BillingService.GetAllBillsAsync
                (organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.bills);
        }
    }
}