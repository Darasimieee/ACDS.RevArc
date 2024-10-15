using System;
using System.Text.Json;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/revenue")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class RevenueController : ControllerBase
    {
        private readonly IServiceManager _service;

        public RevenueController(IServiceManager service) => _service = service;        

        /// <summary>
        /// Gets the list of all revenues 
        /// </summary>
        /// <returns>The revenue list</returns>
        [HttpGet]
        public async Task<IActionResult> GetRevenues([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenueService.GetAllRevenuesAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenues);
        }
        /// <summary>
        /// Gets the list of  revenues by organisationId  
        /// </summary>
        /// <returns>The revenue list</returns>
        [HttpGet("{organisationId}/revenuehead")]
        public async Task<IActionResult> GetRevenuesbyAgencyHead(int organisationId)
        {
            var pagedResult = await _service.RevenueService.GetRevenuesbyHeadAsync(organisationId, trackChanges: false);

           

            return Ok(pagedResult);
        }
        /// <summary>
        /// Refreshes the list of  revenues by agency head from EBS
        /// </summary>
        /// <returns>The revenue list</returns>
        [HttpPost("{organisationId}/fetch-headrevenues")]
        public async Task<IActionResult> FetchheadRevenuesFromEBS(int organisationId)
        {
            var pagedResult = await _service.RevenueService.CreateheadRevenuesAsync(organisationId, trackChanges: false);



            return Ok(pagedResult);
        }
        /// <summary>
        /// Gets a specific Revenues using organisation id 
        /// </summary>
        /// <returns>The Revenues</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}", Name = "revenuesByOrganisationId")]
        public async Task<IActionResult> GetOrganisationsRevenues(int organisationId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenueService.GetOrganisationsRevenueAsync(organisationId, requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenues);
        }
        /// <summary>
        /// Gets a specific Revenues using the businessTypeId 
        /// </summary>
        /// <returns>The Revenues</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/business-type/{businessTypeId:int}", Name = "RevenuesByTypeId")]
        public async Task<IActionResult> GetRevenuesBybusinessType(int organisationId,int businessTypeId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenueService.GetRevenuesbyBusinessTypeAsync(organisationId, businessTypeId, requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenues);
        }
        /// <summary>
        /// Gets a specific Revenues using their id 
        /// </summary>
        /// <returns>The Revenues</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/{revenueId:int}", Name = "RevenuesById")]
        public async Task<IActionResult> GetRevenues(int revenueId)
        {
            var revenue = await _service.RevenueService.GetRevenueAsync(revenueId, trackChanges: false);

            return Ok(revenue);
        }
        /// <summary>
        /// Updates a specific Revenue using their id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("{Id:int}")]
        public async Task<IActionResult> UpdateRevenue(int Id, [FromBody] RevenueUpdateDto revenue)
        {
            if (revenue is null)
                return BadRequest("RevenueUpdateDto object is null");

            await _service.RevenueService.UpdateRevenueAsync(Id, revenue, trackChanges: true);

            return Ok("Revenue sucessfully Updated");
        }

        /// <summary>
        /// Revenue Creation 
        /// </summary>
        /// <returns>Status if revenue is created</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-revenue")]
        public async Task<IActionResult> CreateRevenue([FromBody] RevenueCreationDto revenue)
        {
            if (revenue is null)
                return BadRequest("revenue object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var enumeration = await _service.RevenueService.CreateRevenueAsync(revenue);

            return Ok(enumeration);
        }   
  
        /// <summary>
        /// Category Creation 
        /// </summary>
        /// <returns>Status if Category is created</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategory(CategoryCreationDto category)
        {
            if (category is null)
                return BadRequest("Category object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

             var categoryresponse = await _service.CategoryService.CreateCategoryAsync(category);

            return Ok(categoryresponse);
        }

        /// <summary>
        /// Gets the list of all categories 
        /// </summary>
        /// <returns>The categories list</returns>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategory([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.CategoryService.GetAllCategoriesAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.categories);
        }

        /// <summary>
        /// Gets a specific categories using organisation id 
        /// </summary>
        /// <returns>The categories</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/categories", Name = "categoriesByOrganisationId")]
        public async Task<IActionResult> GetOrganisationsCategories(int organisationId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.CategoryService.GetOrganisationsCategoryAsync(organisationId, requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.categories);
        }

        /// <summary>
        /// Gets a specific category using their id 
        /// </summary>
        /// <returns>The categories</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{categoryId:int}/category", Name = "categoryById")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _service.CategoryService.GetCategoryAsync(categoryId, trackChanges: false);
            return Ok(category);
        }
        /// <summary>
        /// Gets a specific categories using their BusinessSizeId
        /// </summary>
        /// <returns>The categories</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/business-size/{businessSizeId:int}", Name = "categoryBySizeId")]
        public async Task<IActionResult> GetCategoryByBusinessSize(int businessSizeId,int organisationId)
        {
            var category = await _service.CategoryService.GetCategorybyBusinessSize(businessSizeId, organisationId, trackChanges: false);
            return Ok(category);
        }
        /// <summary>
        /// Updates a specific category using their id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("{Id:int}/categories")]
        public async Task<IActionResult> UpdateCategory(int Id, [FromBody] CategoryUpdateDto category)
        {
            if (category is null)
                return BadRequest("CategoryUpdateDto object is null");

            await _service.CategoryService.UpdateCategoryAsync(Id, category, trackChanges: true);

            return Ok("Category sucessfully Updated");
        }
        /// <summary>
        /// RevenueCategory Creation 
        /// </summary>
        /// <returns>Status if RevenueCategory is created</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{revenueId:int}/revenue-category")]
        public async Task<IActionResult> CreateRevenueCategory(int revenueId, RevenueCategoryCreationDto revenuecategory)
        {
            if (revenuecategory is null)
                return BadRequest("RevenueCategory object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var revenuecatresponse = await _service.RevenueCategoryService.CreateRevenueCategoryAsync(revenueId,revenuecategory);

            return Ok(revenuecatresponse);
        }

        /// <summary>
        /// Gets the list of all Revenuecategories 
        /// </summary>
        /// <returns>The Revenuecategories list</returns>
        [HttpGet("revenue-Categories")]
        public async Task<IActionResult> GetRevenueCategory([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenueCategoryService.GetAllRevenueCategoriesAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenueCategories);
        }

        /// <summary>
        /// Gets a specific Revenuecategories using organisation id 
        /// </summary>
        /// <returns>The Revenuecategories</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/revenue-category", Name = "revenuecategoriesByOrganisationId")]
        public async Task<IActionResult> GetOrganisationsRevenueCategories(int organisationId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenueCategoryService.GetOrganisationsRevenueCategoryAsync(organisationId, requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenueCategories);
        }

        /// <summary>
        /// Gets a specific Revenuecategories using their id 
        /// </summary>
        /// <returns>The Revenuecategories</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/revenue-category/{revenuecategoryId:int}", Name = "revenuecategoryById")]
        public async Task<IActionResult> GetRevenueCategory(int revenuecategoryId)
        {
            var revenuecategory = await _service.RevenueCategoryService.GetRevenueCategoryAsync(revenuecategoryId, trackChanges: false);

            return Ok(revenuecategory);
        }

        /// <summary>
        /// Updates a specific Revenuecategory using their id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("revenue-category/{Id:int}")]
        public async Task<IActionResult> UpdateRevenueCategoory(int Id, [FromBody] RevenueCategoryUpdateDto revenuecategory)
        {
            if (revenuecategory is null)
                return BadRequest("RevenueCategoryUpdateDto object is null");

            await _service.RevenueCategoryService.UpdateRevenueCategoryAsync(Id, revenuecategory, trackChanges: true);

            return Ok("RevenueCategory sucessfully Updated");
        }

        /// <summary>
        /// RevenuePrice Creation 
        /// </summary>
        /// <returns>Status if RevenuePrice is created</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("revenue-price/{revenueId:int}")]
        public async Task<IActionResult> CreateRevenuePrice(int revenueId, RevenuePricesCreationDto revenuePrices)
        {

            if (revenuePrices is null)
                return BadRequest("RevenueCategory object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var revenuePriceRes = await _service.RevenuePriceService.CreateRevenuePriceAsync(revenueId, revenuePrices);

            return Ok(revenuePriceRes);
        }

        /// <summary>
        /// Gets the list of all RevenuePrices 
        /// </summary>
        /// <returns>The RevenuePrices list</returns>
        [HttpGet("revenue-prices")]
        public async Task<IActionResult> GetRevenuePrices([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenuePriceService.GetAllRevenuePricesAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenuePrices);
        }

        /// <summary>
        /// Gets  RevenuePrices using organisation id 
        /// </summary>
        /// <returns>The RevenuePrices</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("revenue-price/{organisationId:int}", Name = "revenuepricesByOrganisationId")]
        public async Task<IActionResult> GetOrganisationsRevenuePrices(int organisationId,[FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RevenuePriceService.GetOrganisationsRevenuePricesAsync(organisationId, requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.revenuePriceList);
        }

        /// <summary>
        /// Gets a specific RevenuePrice using their id 
        /// </summary>
        /// <returns>The RevenuePrice</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/revenue-price/{revenuePriceId:int}", Name = "revenuepriceById")]
        public async Task<IActionResult> GetRevenuePriceCategory(int revenuePriceId)
        {
            var revenuePrice = await _service.RevenuePriceService.GetRevenuePriceAsync(revenuePriceId, trackChanges: false);

            return Ok(revenuePrice);
        }
        /// <summary>
        /// Gets a  RevenuePrices using revenue id 
        /// </summary>
        /// <returns>The RevenuePrice</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/revenueprice-revenue/{revenueId:int}")]
        public async Task<IActionResult> xGetRevenuePriceRevenueId(int revenueId)
        {
            var revenuePrice = await _service.RevenuePriceService.GetRevenuePriceByRevenueAsync(revenueId, trackChanges: false);

            return Ok(revenuePrice);    
        }
        /// <summary>
        /// Gets a specific RevenuePrice using categoryid 
        /// </summary>
        /// <returns>The RevenuePrice</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("{organisationId:int}/revenueprice-category/{categoryId:int}")]
        public async Task<IActionResult> GetRevenuePriceCategory(int organisationId, int categoryId, [FromQuery] RoleParameters requestParameters)
        {
            var revenuePrice = await _service.RevenuePriceService.GetCategorysRevenuePricesAsync(organisationId, categoryId, requestParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(revenuePrice.metaData));

            return Ok(revenuePrice.revenuePrices);

        }

        /// <summary>
        /// Updates a specific RevenuePrices using their id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("{Id:int}/revenue-price")]
        public async Task<IActionResult> UpdateRevenuePrice(int Id, [FromBody] RevenuePricesUpdateDto revenuePrice)
        {
            if (revenuePrice is null)
                return BadRequest("RevenuePriceUpdateDto object is null");

            await _service.RevenuePriceService.UpdateRevenuePriceAsync(Id, revenuePrice, trackChanges: true);

            return Ok("Revenue Price sucessfully Updated");
        }  
    }
}