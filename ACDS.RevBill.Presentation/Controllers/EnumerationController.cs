using System.Data;
using System.Net;
using System.Text.Json;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Agencies;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/enumeration")]
    [ApiController]
    [Produces("application/json")]
    public class EnumerationController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly ILoggerManager _loggerManager;

        public EnumerationController(IServiceManager service, ILoggerManager loggerManager)
        {
            _service = service;
            _loggerManager = loggerManager;

        }

        /// <summary>
        /// Gets the list of all genders
        /// </summary>
        /// <returns>The genders list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("genders")]
        public async Task<IActionResult> GetGenders()
        {
            var genders = await _service.EnumerationService.GetAllGendersAsync(trackChanges: false);

            return Ok(genders);
        }
        /// <summary>
        /// Gets the refresh the list of bankcodes
        /// </summary>
        /// <returns>The success</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("bankcodes")]
        public async Task<IActionResult> GetBankcodes()
        {
            var response = await _service.EnumerationService.PushBankcodeAsync();

            return Ok(response);
        }
        /// <summary>
        /// Gets  list of  banks
        /// </summary>
        /// <returns>The success</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("banks")]
        public async Task<IActionResult> GetBanks()
        {
            var response = await _service.EnumerationService.GetAllBanksAsync();

            return Ok(response);
        }
        /// <summary>
        /// Gets the list of all titles
        /// </summary>
        /// <returns>The titles list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("titles")]
        public async Task<IActionResult> GetTitles()
        {
            var titles = await _service.EnumerationService.GetAllTitlesAsync(trackChanges: false);

            return Ok(titles);
        }

        /// <summary>
        /// Gets the list of all marital statuses
        /// </summary>
        /// <returns>The marital status list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("marital-statuses")]
        public async Task<IActionResult> GetMaritalStatuses()
        {
            var maritalStatuses = await _service.EnumerationService.GetAllMaritalStatusAsync(trackChanges: false);

            return Ok(maritalStatuses);
        }

        /// <summary>
        /// Gets the list of all payer types
        /// </summary>
        /// <returns>The payer types list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("payer-types")]
        public async Task<IActionResult> GetPayerTypes()
        {
            var payerTypes = await _service.EnumerationService.GetAllPayerTypesAsync(trackChanges: false);

            return Ok(payerTypes);
        }

        /// <summary>
        /// Gets the list of all lgas by state
        /// </summary>
        /// <returns>The LGAs list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{stateId}/lgas")]
        public async Task<IActionResult> GetLgasByState(int stateId)
        {
            var lgas = await _service.EnumerationService.GetLgasByStateAsync(stateId, trackChanges: false);

            return Ok(lgas);
        }

        /// <summary>
        /// Gets all lcdas under an Lga
        /// </summary>
        /// <returns>The LCDAs list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{lgaId}/lcdas")]
        public async Task<IActionResult> GetLcdasByLga(int lgaId)
        {
            var lcdas = await _service.EnumerationService.GetLcdasByLgaAsync(lgaId, trackChanges: false);

            return Ok(lcdas);
        }

        /// <summary>
        /// Gets the list of all states
        /// </summary>
        /// <returns>The States list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var states = await _service.EnumerationService.GetAllStatesAsync(trackChanges: false);

            return Ok(states);
        }

        /// <summary>
        /// Gets the list of all countries
        /// </summary>
        /// <returns>The Countries list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _service.EnumerationService.GetAllCountriesAsync(trackChanges: false);

            return Ok(countries);
        }

        /// <summary>
        /// Gets the list of all Properties
        /// </summary>
        /// <returns>The Properties list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("{organisationId}/property")]
        //public async Task<IActionResult> GetProperties(int organisationId, [FromQuery] PropertyParameters requestParameters)
        //{
        //    var pagedResult = await _service.EnumerationService.GetAllPropertiesAsync(organisationId, requestParameters, trackChanges: false);

        //    Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        //    return Ok(pagedResult.properties);
        //}

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId}/property")]
        public async Task<IActionResult> GetProperties(int organisationId, [FromQuery] PropertyParameters requestParameters)
        {
            var pagedResult = await _service.EnumerationService.GetAllPropertiesAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.properties);
        }

        /// <summary>
        /// Gets the list of all Properties by agency Id
        /// </summary>
        /// <returns>The Properties list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId}/property/{agencyId}/agency")]
        public async Task<IActionResult> GetPropertiesbyAgency(int organisationId,int agencyId, [FromQuery] PropertyParameters requestParameters)
        {
            var pagedResult = await _service.EnumerationService.GetPropertiesbyAgencyAsync(organisationId, agencyId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.properties);
        }
        /// <summary>
        /// Gets a specific Property using their id
        /// </summary>
        /// <returns>The Property</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/property/{id:int}", Name = "PropertyById")]
        public async Task<IActionResult> GetProperty(int organisationId, int id)
        {
            var property = await _service.EnumerationService.GetPropertyAsync(organisationId, id, trackChanges: false);

            return Ok(property);
        }

        /// <summary>
        /// Creates a new Property
        /// </summary>
        /// <returns>Property Successfully Created</returns>
        /// <response code="201">Property Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId}/property")]
        public async Task<IActionResult> CreateProperty([FromRoute] int organisationId, [FromBody] CreatePropertyDto createProperty)
        {
            if (createProperty is null)
                return BadRequest("CreatePropertyDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
           
            var createdProperty = await _service.EnumerationService.CreatePropertyAsync(organisationId, createProperty);

            return Ok(createdProperty) ;
        }

        /// <summary>
        /// Updates a specific property using their id
        /// </summary>
        /// <response code="204">Successfully Updated Property</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/property-update/{id:int}")]
        public async Task<IActionResult> UpdateProperty(int organisationId, int id, [FromBody] PropertyUpdateDto property)
        {
            if (property is null)
                return BadRequest("PropertyUpdateDto  object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EnumerationService.UpdatePropertyAsync(organisationId, id, property, trackChanges: true);

            return Ok(new
            {
                message = ($"Property succesfully updated")
            });
        }

        /// <summary>
        /// Gets the list of all wards
        /// </summary>
        /// <returns>The wards list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("{organisationId:int}/wards", Name = "WardById")]
        //public async Task<IActionResult> GetAllWards(int organisationId)
        //{
        //    var wards = await _service.EnumerationService.GetAllWardsAsync(organisationId, trackChanges: false);

        //    return Ok(wards);
        //}

        /// <summary>
        /// Get ward by Id
        /// </summary>
        /// <returns>The ward</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("{organisationId:int}/wards/{id:int}")]
        //public async Task<IActionResult> GetWard(int organisationId, int id)
        //{
        //    var wards = await _service.EnumerationService.GetWardAsync(organisationId, id, trackChanges: false);

        //    return Ok(wards);
        //}

        ///// <summary>
        ///// Create ward 
        ///// </summary>
        ///// <returns>Created ward</returns>
        ///// <response code="200">Successful</response>
        ///// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPost("{organisationId:int}/wards")]
        //public async Task<IActionResult> CreateWard(int organisationId, [FromBody] CreateWardDto createWard)
        //{
        //    if (createWard is null)
        //        return BadRequest("CreateWardDto object is null");

        //    if (!ModelState.IsValid)
        //        return UnprocessableEntity(ModelState);

        //    var wards = await _service.EnumerationService.CreateWardAsync(organisationId, createWard);

        //    return Ok(new
        //    {
        //        message = ($"Ward succesfully created")
        //    });
        //}

        /// <summary>
        /// Updates a ward using their id
        /// </summary>
        /// <response code="200">Successfully Updated ward</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPost("{organisationId:int}/wards/{id:int}")]
        //public async Task<IActionResult> UpdateWard(int organisationId, int id, [FromBody] UpdateWardDto updateWard)
        //{
        //    if (updateWard is null)
        //        return BadRequest("UpdateWardDto object is null");

        //    if (!ModelState.IsValid)
        //        return UnprocessableEntity(ModelState);

        //    await _service.EnumerationService.UpdateWardAsync(organisationId, id, updateWard, trackChanges: true);

        //    return Ok(new
        //    {
        //        message = ($"Ward with ID:{id} succesfully updated")
        //    });
        //}

        /// <summary>
        /// Gets the list of all space identifiers
        /// </summary>
        /// <returns>The space identifiers list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/space-identifiers", Name = "SpaceIdentifierById")]
        public async Task<IActionResult> GetAllSpaceIdentifiers(int organisationId)
        {
            var spaceIdentifiers = await _service.EnumerationService.GetAllSpaceIdentifiersAsync(organisationId, trackChanges: false);

            return Ok(spaceIdentifiers);
        }

        /// <summary>
        /// Gets space identifier
        /// </summary>
        /// <returns>The space identifier</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/space-identifiers/{id:int}")]
        public async Task<IActionResult> GetSpaceIdentifiers(int organisationId, int id)
        {
            var spaceIdentifiers = await _service.EnumerationService.GetSpaceIdentifierAsync(organisationId, id, trackChanges: false);

            return Ok(spaceIdentifiers);
        }

        /// <summary>
        /// Create Space Identifier
        /// </summary>
        /// <returns>Created space identifier</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/space-identifiers")]
        public async Task<IActionResult> CreateSpaceIdentifier(int organisationId, [FromBody] CreateSpaceIdentifierDto createSpaceIdentifier)
        {
            if (createSpaceIdentifier is null)
                return BadRequest("CreateSpaceIdentifierDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var spaceIdentifier = await _service.EnumerationService.CreateSpaceIdentifierAsync(organisationId, createSpaceIdentifier);

            return Ok(new
            {
                message = ($"Space Identifier succesfully created")
            });
        }

        /// <summary>
        /// Updates a Space Identifier using their id
        /// </summary>
        /// <response code="200">Successfully Updated ward</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/space-identifiers/{id:int}")]
        public async Task<IActionResult> UpdateSpaceIdentifier(int organisationId, int id, [FromBody] UpdateSpaceIdentifierDto updateSpaceIdentifier)
        {
            if (updateSpaceIdentifier is null)
                return BadRequest("UpdateSpaceIdentifierDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EnumerationService.UpdateSpaceIdentifierAsync(organisationId, id, updateSpaceIdentifier, trackChanges: true);

            return Ok(new
            {
                message = ($"Space Identifier with ID:{id} succesfully updated")
            });
        }

        /// <summary>
        /// Gets the list of all business types
        /// </summary>
        /// <returns>The business types list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/business-types", Name = "BusinessTypeById")]
        public async Task<IActionResult> GetAllBusinessTypes(int organisationId)
        {
            var businessTypes = await _service.EnumerationService.GetAllBusinessTypesAsync(organisationId, trackChanges: false);

            return Ok(businessTypes);
        }

        /// <summary>
        /// Gets business type by id
        /// </summary>
        /// <returns>The business type</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/business-types/{id:int}")]
        public async Task<IActionResult> GetBusinessType(int organisationId, int id)
        {
            var businessTypes = await _service.EnumerationService.GetBusinessTypeAsync(organisationId, id, trackChanges: false);

            return Ok(businessTypes);
        }

        /// <summary>
        /// Create Business Type
        /// </summary>
        /// <returns>Created business type</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/business-types")]
        public async Task<IActionResult> CreateBusinessType(int organisationId, [FromBody] CreateBusinessTypeDto createBusinessType)
        {
            if (createBusinessType is null)
                return BadRequest("CreateBusinessTypeDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var businessType = await _service.EnumerationService.CreateBusinessTypeAsync(organisationId, createBusinessType);

            return Ok(new
            {
                message = ($"Business type succesfully created")
            });
        }

        /// <summary>
        /// Updates a business type using their id
        /// </summary>
        /// <response code="200">Successfully Updated ward</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/business-types/{id:int}")]
        public async Task<IActionResult> UpdateBusinessType(int organisationId, int id, [FromBody] UpdateBusinessTypeDto updateBusinessType)
        {
            if (updateBusinessType is null)
                return BadRequest("UpdateBusinessTypeDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EnumerationService.UpdateBusinessTypeAsync(organisationId, id, updateBusinessType, trackChanges: true);

            return Ok(new
            {
                message = ($"Business type with ID:{id} succesfully updated")
            });
        }

        /// <summary>
        /// Gets the list of all business sizes
        /// </summary>
        /// <returns>The business sizes list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/business-sizes", Name = "BusinessSizeById")]
        public async Task<IActionResult> GetAllBusinessSizes(int organisationId)
        {
            var businessSizes = await _service.EnumerationService.GetAllBusinessSizesAsync(organisationId, trackChanges: false);

            return Ok(businessSizes);
        }

        /// <summary>
        /// Gets business size by id
        /// </summary>
        /// <returns>The business type</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/business-sizes/{id:int}")]
        public async Task<IActionResult> GetBusinessSize(int organisationId, int id)
        {
            var businessSize = await _service.EnumerationService.GetBusinessSizeAsync(organisationId, id, trackChanges: false);

            return Ok(businessSize);
        }

        /// <summary>
        /// Create Business Size
        /// </summary>
        /// <returns>Created business size</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/business-sizes")]
        public async Task<IActionResult> CreateBusinessSize(int organisationId, [FromBody] CreateBusinessSizeDto createBusinessSize)
        {
            if (createBusinessSize is null)
                return BadRequest("CreateBusinessSizeDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var businessSize = await _service.EnumerationService.CreateBusinessSizeAsync(organisationId, createBusinessSize);

            return Ok(new
            {
                message = ($"Business size succesfully created")
            });
        }

        /// <summary>
        /// Updates a business size using their id
        /// </summary>
        /// <response code="200">Successfully Updated ward</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/business-sizes/{id:int}")]
        public async Task<IActionResult> UpdateBusinessSize(int organisationId, int id, [FromBody] UpdateBusinessSizeDto updateBusinessSize)
        {
            if (updateBusinessSize is null)
                return BadRequest("UpdateBusinessSizeDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EnumerationService.UpdateBusinessSizeAsync(organisationId, id, updateBusinessSize, trackChanges: true);

            return Ok(new
            {
                message = ($"Business size with ID:{id} succesfully updated")
            });
        }

        /// <summary>
        /// Gets the list of all business profiles
        /// </summary>
        /// <returns>The business profiles list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/business-profiles", Name = "BusinessProfilesById")]
        public async Task<IActionResult> GetAllBusinessProfiles(int organisationId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.EnumerationService.GetAllBusinessProfilesAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.businessProfile);
        }

        /// <summary>
        /// Gets business profile by id
        /// </summary>
        /// <returns>The business profile</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/business-profiles/{id:int}")]
        public async Task<IActionResult> GetBusinessProfile(int organisationId, int id)
        {
            var businessSize = await _service.EnumerationService.GetBusinessProfileAsync(organisationId, id, trackChanges: false);

            return Ok(businessSize);
        }

        /// <summary>
        /// Complete Enumeration Process, from Property to Customer
        /// </summary>
        /// <returns>Successfully Enumerated</returns>
        /// <response code="200">Property Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId}")]
        public async Task<IActionResult> Enumeration([FromRoute]int organisationId,[FromBody] CompleteEnumerationParams enumerate)
        {
            if (enumerate is null)
                return BadRequest("CompleteEnumerationParams object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.EnumerationService.EnumerationAsync(organisationId, enumerate, trackChanges: false);

            return Ok(enumeration);
        }

        /// <summary>
        /// Partial Enumeration Process, from Business profile to Customer
        /// </summary>
        /// <returns>Successfully Enumerated</returns>
        /// <response code="200">Property Successfully Created</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId}/property/{propertyId}")]
        public async Task<IActionResult> PartialEnumeration(int organisationId, int propertyId, [FromBody] PartialEnumerationParams enumerate)
        {
            if (enumerate is null)
                return BadRequest("PartialEnumerationParams object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.EnumerationService.EnumerationWhenPropertyExistsAsync(organisationId, propertyId, enumerate, trackChanges: false);

            return Ok(enumeration);
        }

        /// <summary>
        /// Search for payer id by phone number
        /// </summary>
        /// <returns>Enumeration status</returns>
        /// <response code="200">Returned payer id details</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("pid-search-by-phonenumber")]
        public IActionResult SearchForPayerIDByPhoneNumber(GetTaxPayerRequestDto getTaxPayer)
        {
            if (getTaxPayer is null)
                return BadRequest("GetTaxPayerRequestDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var search = _service.EnumerationService.PayerIdSearchByPhoneNumber(getTaxPayer);

            return Ok(search);
        }

        /// <summary>
        /// Search for payer id by name
        /// </summary>
        /// <returns>Enumeration status</returns>
        /// <response code="200">Returned payer id details</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("pid-search-by-name")]
        public IActionResult SearchForPayerIDByName(GetTaxPayerRequestDto getTaxPayer)
        {
            if (getTaxPayer is null)
                return BadRequest("GetTaxPayerRequestDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var search = _service.EnumerationService.PayerIdSearchByName(getTaxPayer);

            return Ok(search);
        }

        /// <summary>
        /// Search for payer id by email
        /// </summary>
        /// <returns>Enumeration status</returns>
        /// <response code="200">Returned payer id details</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("pid-search-by-email")]
        public IActionResult SearchForPayerIDByEmail(GetTaxPayerRequestDto getTaxPayer)
        {
            if (getTaxPayer is null)
                return BadRequest("GetTaxPayerRequestDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var search = _service.EnumerationService.PayerIdSearchByEmail(getTaxPayer);

            return Ok(search);
        }

        /// <summary>
        /// Verify Payer Id
        /// </summary>
        /// <returns>Status if PID exists</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("verify-pid")]
        public IActionResult VerifyPid(PayerIdEnumerationDto customer)
        {
            if (customer is null)
                return BadRequest("customer object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = _service.EnumerationService.VerifyPid(customer);

            return Ok(enumeration);
        }
        /// <summary>
        /// Verify Agencycode 
        /// </summary>
        /// <returns>Status if Agencycode exists</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{agencycode}/verify-agencycode")]
        public IActionResult VerifyAgencyCode(string agencycode)
        {
            if (agencycode is null)
                return BadRequest("agency code object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = _service.EnumerationService.VerifyAgencyCode(agencycode);

            return Ok(enumeration);
        }
        /// <summary>
        /// Customer enumeration by NIN
        /// </summary>
        /// <returns>Generated PID</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-pid-nin")]
        public async Task<IActionResult> CustomerEnumerationByNIN(CustomerEnumerationNINDto customer)
        {
            if (customer is null)
                return BadRequest("customer object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.EnumerationService.CreatePIDWithNIN(customer);

            return Ok(enumeration);
        }

        /// <summary>
        /// Customer enumeration by BVN
        /// </summary>
        /// <returns>Generated PID</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-pid-bvn")]
        public async Task<IActionResult> CustomerEnumerationByBVN(CustomerEnumerationBVNDto customer)
        {
            if (customer is null)
                return BadRequest("customer object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.EnumerationService.CreatePIDWithBVN(customer);

            return Ok(enumeration);
        }

        /// <summary>
        /// Customer enumeration by BioData
        /// </summary>
        /// <returns>Generated PID</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-pid-biodata")]
        public async Task<IActionResult> CustomerEnumerationWithoutBVNOrNIN(CustomerEnumerationDto customer)
        {
            _loggerManager.LogInfo($"Starting customer eumeration for customer {customer.Email}");
            if (customer is null)
                return BadRequest("customer object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.EnumerationService.CreatePIDWithBioData(customer);
            _loggerManager.LogInfo($"ending customer eumeration for customer {customer.Email}");
            return Ok(enumeration);
        }

        /// <summary>
        /// Corporate PayerID Creation
        /// </summary>
        /// <returns>Generated PID</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-pid-corporate")]
        public IActionResult CorporatePayerIDCreation(CorporatePayerIDRequest customer)
        {
            if (customer is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = _service.EnumerationService.CreateCorporatePID(customer);

            return Ok(enumeration);
        }

        /// <summary>
        /// Gets the list of all agencies 
        /// </summary>
        /// <returns>The agency list</returns>
        /// /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("agencies")]
        public async Task<IActionResult> GetAgencies([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.AgencyService.GetAllAgenciesAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.agencies);
        }
        /// <summary>
        /// Gets a specific Agencies in an organisation using organisationid 
        /// </summary>
        /// <returns>The Agency</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/agencies", Name = "AgencyByOrganisationId")]
        public async Task<IActionResult> GetOrganiisationsAgency(int organisationId, [FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.AgencyService.GetOrganisationsAgencyAsync(organisationId, roleParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.agencies);
        }
        /// <summary>
        /// Gets a specific Agency using their id 
        /// </summary>
        /// <returns>The Agency</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/agencies/{id:int}", Name = "AgencyById")]
        public async Task<IActionResult> GetAgency(int id)
        {
            var agency = await _service.AgencyService.GetAgencyAsync(id, trackChanges: false);

            return Ok(agency);
        }

        /// <summary>
        /// Updates a specific Agency using their id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("agencies/{id:int}")]
        public async Task<IActionResult> UpdateAgency(int id, [FromBody] AgencyUpdateDto agency)
        {
            if (agency is null)
                return BadRequest("AgencyUpdateDto object is null");

            await _service.AgencyService.UpdateAgencyAsync(id, agency, trackChanges: true);

            return Ok(new
            {
                message = "Agency sucessfully updated",
            });
        }

        /// <summary>
        /// Agency Creation 
        /// </summary>
        /// <returns>Status if agency is created</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-agency")]
        public async Task<IActionResult> CreateAgency(AgencyCreationDto agency)
        {
            if (agency is null)
                return BadRequest("agency object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.AgencyService.CreateAgencyAsync(agency);

            return Ok(enumeration);
        }
        /// <summary>
        /// Refreshes the list of  agency by agency head from EBS
        /// </summary>
        /// <returns>The revenue list</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId}/fetch-agency")]
        public async Task<IActionResult> FetchAgencyFromEBS(int organisationId)
        {
            if (organisationId is 0)
                return BadRequest("agency object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.AgencyService.FetchAgencyAsync(organisationId);

            return Ok(enumeration);
        }
        /// <summary>
        /// Street Creation 
        /// </summary>
        /// <returns>Status if street is created</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-street")]
        public async Task<IActionResult> CreateStreet(BulkStreetCreation street)
        {
            if (street is null)
                return BadRequest("street object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var result = await _service.StreetService.CreateStreetAsync(street);

            return Ok(result);
        }
        /// <summary>
        /// Gets the list of all streets 
        /// </summary>
        /// <returns>The streets list</returns>
        /// /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("streets")]
        public async Task<IActionResult> GetStreets([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.StreetService.GetAllStreetsAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.street);
        }
        /// <summary>
        /// Gets a list of Streets in an organisation using organisationid 
        /// </summary>
        /// <returns>The Streets</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/streets", Name = "StreetByOrganisationId")]
        public async Task<IActionResult> GetOrganiisationsStreets(int organisationId, [FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.StreetService.GetOrganisationsStreetAsync(organisationId, roleParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.street);
        }
        /// <summary>
        /// Gets a list of Streets in an agency using organisationid and orgId 
        /// </summary>
        /// <returns>The Streets</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/agency/{agencyId:int}/streets", Name = "StreetByagencyandOrganisationId")]
        public async Task<IActionResult> GetAgencyStreets(int organisationId,int agencyId, [FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.StreetService.GetAgencyStreetAsync(organisationId, agencyId, roleParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.street);
        }
        /// <summary>
        /// Gets a specific Streets using its id 
        /// </summary>
        /// <returns>The Street</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/streets/{id:int}")]
        public async Task<IActionResult> GetStreet(int id)
        {
            var street = await _service.StreetService.GetStreetAsync(id, trackChanges: false);

            return Ok(street);
        }

        /// <summary>
        /// Updates a specific Street using its id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("streets/{id:int}")]
        public async Task<IActionResult> UpdateStreet(int id, [FromBody] StreetUpdateDto street)
        {
            if (street is null)
                return BadRequest("StreetsUpdateDto object is null");

            await _service.StreetService.UpdateStreetAsync(id, street, trackChanges: true);

            return Ok(new
            {
                message = "Street sucessfully updated",
            });
        }
        /// <summary>
        /// Count of all registered properties this current month
        /// </summary>
        /// <returns>Count of all registered properties this current month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-properties-this-month")]
        public async Task<IActionResult> PropertyCountThisMonth(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredPropertiesThisMonth(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered properties this current week
        /// </summary>
        /// <returns>Count of all registered properties this current week</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-properties-this-week")]
        public async Task<IActionResult> PropertyCountThisWeek(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredPropertiesThisWeek(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered properties today
        /// </summary>
        /// <returns>Count of all registered properties today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-properties-today")]
        public async Task<IActionResult> PropertyCountToday(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredPropertiesToday(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered non-properties this current month
        /// </summary>
        /// <returns>Count of all registered non-properties this current month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-non-properties-this-month")]
        public async Task<IActionResult> NonPropertyCountThisMonth(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredNonPropertiesThisMonth(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered non-properties this current week
        /// </summary>
        /// <returns>Count of all registered non-properties this current week</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-non-properties-this-week")]
        public async Task<IActionResult> NonPropertyCountThisWeek(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredNonPropertiesThisWeek(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered non-properties today
        /// </summary>
        /// <returns>Count of all registered non-properties today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-non-properties-today")]
        public async Task<IActionResult> NonPropertyCountToday(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredNonPropertiesToday(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered customers this current month
        /// </summary>
        /// <returns>Count of all registered customers this current month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-customers-this-month")]
        public async Task<IActionResult> CustomerCountThisMonth(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredCustomersThisMonth(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered customers this current week
        /// </summary>
        /// <returns>Count of all registered customerss this current week</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-customers-this-week")]
        public async Task<IActionResult> CustomerCountThisWeek(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredCustomersThisWeek(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered customers today
        /// </summary>
        /// <returns>Count of all registered customers today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-registered-customers-today")]
        public async Task<IActionResult> CustomerCountToday(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfRegisteredCustomersToday(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all registered customers and properties by agency
        /// </summary>
        /// <returns>Count of all registered customers and properties by agency</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-properties-and-customers-by-area-office")]
        public async Task<IActionResult> CustomerAndPropertyCountByAreaOffice(int organisationId)
        {
            var count = await _service.EnumerationService.NoOfPropertiesAndCustomersByAreaOffice(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Enumeration manifest
        /// </summary>
        /// <returns>Enumeration manifest</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/manifest")]
        public async Task<IActionResult> EnumerationManifest(int organisationId, [FromQuery] PaginationFilter filter)
        {
            var manifest = await _service.EnumerationService.EnumerationManifest(organisationId, filter);

            return Ok(manifest);
        }

        /// <summary>
        /// Enumeration manifest by ID
        /// </summary>
        /// <returns>Enumeration manifest</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/manifest/{id:int}")]
        public async Task<IActionResult> EnumerationManifestByID(int organisationId, int id)
        {
            var manifest = await _service.EnumerationService.EnumerationManifestById(organisationId, id);

            return Ok(manifest);
        }

        /// <summary>
        /// Remove Customer from property
        /// </summary>
        /// <returns>Customer removed from property</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/remove-customer-from-property/property/{propertyId:int}/customer/{customerId:int}")]
        public async Task<IActionResult> RemoveCustomerFromProperty(int organisationId, int propertyId, int customerId)
        {
            await _service.EnumerationService.RemoveCustomerFromProperty(organisationId, propertyId, customerId, trackChanges: false);

            return Ok(new
            {
                message = "Customer sucessfully removed from property",
            });
        }

        /// <summary>
        /// Gets the list of all customer properties
        /// </summary>
        /// <returns>The customer properties list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId}/customer-properties/property/{propertyId:int}")]
        public async Task<IActionResult> GetCustomerProperties(int organisationId, int propertyId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.EnumerationService.GetAllCustomerPropertiesAsync(organisationId, propertyId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.properties);
        }
        /// <summary>
        /// Upload streets
        /// </summary>
        /// <returns>Success message of streets created </returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId}/agency/{agencyid:int}/uploadStreets")]
        public async Task<IActionResult> UploadStreets(int organisationId,int agencyid, CancellationToken ct)
        {

            if (Request.Form.Files.Count == 0) 
                return BadRequest("No content!");

            var file = Request.Form.Files[0];
            var creator = Request.Form["createdby"];
           var pagedResult = await _service.StreetService.UploadStreetAsync(organisationId, agencyid, creator, file);

           
           return Ok(pagedResult);
        }
    }
}