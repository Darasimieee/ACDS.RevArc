using System;
using System.Text.Json;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/customer")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CustomerController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all customers
        /// </summary>
        /// <returns>The customers list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}")]
        public async Task<IActionResult> GetCustomers(int organisationId, [FromQuery] CustomerParameters requestParameters)
        {
            var pagedResult = await _service.CustomerService.GetAllCustomersAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.customer);
        }

        /// <summary>
        /// Gets a specific customer using their id
        /// </summary>
        /// <returns>The customer</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/{id:int}", Name = "CustomerById")]
        public async Task<IActionResult> GetCustomer(int organisationId, int id)
        {
            var customer = await _service.CustomerService.GetCustomerAsync(organisationId, id, trackChanges: false);

            return Ok(customer);
        }

        /// <summary>
        /// Gets a specific customer using their email
        /// </summary>
        /// <returns>The customer</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("email/{email}", Name = "CustomerByEmail")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            var customer = await _service.CustomerService.GetCustomerByEmailAsync(email, trackChanges: false);

            return Ok(customer);
        }

        /// <summary>
        /// Creates a new customer without a property
        /// </summary>
        /// <returns>Successful customer creation</returns>
        /// <response code="201">Customer Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}")]
        public async Task<IActionResult> CreateCustomerWithoutProperty(int organisationId, [FromBody] CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto is null)
                return BadRequest("createCustomerDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var createdCustomer = await _service.CustomerService.CreateCustomerWithoutPropertyAsync(organisationId, createCustomerDto);

            return Ok(createdCustomer);
        }

        /// <summary>
        /// Updates a specific customer using their id
        /// </summary>
        /// <response code="204">Successfully Updated Customer</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/{id:int}")]
        public async Task<IActionResult> UpdateCustomer(int organisationId, int id, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            if (updateCustomerDto is null)
                return BadRequest("updateCustomerDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.CustomerService.UpdateCustomerAsync(organisationId, id, updateCustomerDto, trackChanges: true);

            return NoContent();
        }
    }
}