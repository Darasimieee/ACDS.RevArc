using System.Text.Json;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Payment;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/payment")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        private readonly IServiceManager _service;

        public PaymentController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets an Organisation Payment History
        /// </summary>
        /// <returns>The payment history list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/history")]
        public async Task<IActionResult> GetOrganisationPaymentHistories(int organisationId, [FromQuery] PaymentParameters requestParameters)
        {
            var pagedResult = await _service.PaymentService.GetOrganisationPaymentsAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.payment);
        }
        /// <summary>
        /// Gets an Organisation Payments by parameter
        /// </summary>
        /// <returns>The payment list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/payment-filter")]
        public async Task<IActionResult> GetPaymentByParameter(int organisationId, [FromQuery] PaymentParameters requestParameters)
        {
            var pagedResult = await _service.PaymentService.GetOrganisationPaymentsAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.payment);
        }
        /// <summary>
        /// Gets an Organisation daily Payment by bank
        /// </summary>
        /// <returns>The daily Payment by bank </returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/daily-payment-by-bank")]
        public async Task<IActionResult> GetOrganisationDailyPaymentByBank(int organisationId)
        {
            var count = await _service.PaymentService.GetOrganisationDailyPaymentByBankAsync(organisationId);


            return Ok(count);
        }
        /// <summary>
        /// Gets an Organisation Payment count by Agency
        /// </summary>
        /// <returns>The payment count by Agency </returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-properties-by-agency")]
        public async Task<IActionResult> GetOrganisationPaymentByAgency(int organisationId)
        {
            var count = await _service.PaymentService.GetOrganisationPaymentByAgencyAsync(organisationId);


            return Ok(count);
        }
        ///// <summary>
        ///// Gets an Organisation Payment count by Agency
        ///// </summary>
        ///// <returns>The payment count by Agency </returns>
        ///// <response code="200">Successful</response>
        ///// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("{organisationId:int}/count-properties-by-agency")]
        //public async Task<IActionResult> GetOrganisationPaymentByAgency(int organisationId)
        //{
        //    var count = await _service.PaymentService.GetOrganisationPaymentByAgencyAsync(organisationId);


        //    return Ok(count);
        //}
        /// <summary>
        /// Gets an Organisation Payment count by revenue
        /// </summary>
        /// <returns>The payment count by revenue list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-properties-by-revenue")]
        public async Task<IActionResult> GetOrganisationPaymentByRevenue(int organisationId)
        {
            var count = await _service.PaymentService.GetOrganisationPaymentByRevenueAsync(organisationId);


            return Ok(count);
        }
        ///// <summary>
        ///// Gets an Organisation Daily Payment  by bank
        ///// </summary>
        ///// <returns>The Daily Payment  by bank</returns>
        ///// <response code="200">Successful</response>
        ///// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("{organisationId:int}/daily-payment-by-bank")]
        //public async Task<IActionResult> GetOrganisationDailyPaymentByBank(int organisationId)
        //{
        //    var count = await _service.PaymentService.GetOrganisationDailyPaymentByBankAsync(organisationId);


        //    return Ok(count);
        //}
        /// <summary>
        /// Gets an Organisation Payment count by bank
        /// </summary>
        /// <returns>The payment count by bank list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-properties-by-bank")]
        public async Task<IActionResult> GetOrganisationPaymentByBank(int organisationId)
        {
            var count = await _service.PaymentService.GetOrganisationPaymentByBankAsync(organisationId);


            return Ok(count);
        }
        /// <summary>
        /// Get payment history by id in an Organisation
        /// </summary>
        /// <returns>The payment made</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/histories/{paymentId:long}")]
        public async Task<IActionResult> GetPaymentForOrganisation(int organisationId, long paymentId)
        {
            var payment = await _service.PaymentService.GetPaymentInOrganisationAsync(organisationId, paymentId, trackChanges: false);

            return Ok(payment);
        }

        /// <summary>
        /// Add payment record
        /// </summary>
        /// <returns>The payment made</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("add-payment")]
        public async Task<IActionResult> AddPaymentHistory([FromBody] CreatePaymentDto paymentDto)
        {
            if (paymentDto is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var payment = await _service.PaymentService.AddPaymentHistoryAsync(paymentDto);

            return Ok(payment);
        }

        /// <summary>
        /// Get all individual payment history
        /// </summary>
        /// <returns>The payment made</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("user/{userId:int}/history")]
        public async Task<IActionResult> GetAllIndividualPaymentHistory(int userId)
        {
            var payment = await _service.PaymentService.GetAllIndividualPaymentHistoriesAsync(userId);

            return Ok(payment);
        }

        /// <summary>
        /// Get individual payment history
        /// </summary>
        /// <returns>The payment made</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("user/{userId:int}/history/{paymentId:long}")]
        public async Task<IActionResult> GetIndividualPaymentHistory(int userId, long paymentId)
        {
            var payment = await _service.PaymentService.GetIndividualPaymentHistoryAsync(userId, paymentId);

            return Ok(payment);
        }

        /// <summary>
        /// Gets all payment gateways
        /// </summary>
        /// <returns>The payment gateways list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("gateway")]
        public async Task<IActionResult> GetAllPaymentGateways([FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.PaymentService.GetAllPaymentGatewaysAsync(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.banks);
        }

        /// <summary>
        /// Get payment gateway by id      
        /// </summary>
        /// <returns>The payment banks list by id</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("gateway/{id:int}")]
        public async Task<IActionResult> GetPaymentGateway(int id)
        {
            var bank = await _service.PaymentService.GetPaymentGatewayAsync(id, trackChanges: false);

            return Ok(bank);
        }

        /// <summary>
        /// Add payment gateways 
        /// </summary>
        /// <returns>Created payment gateway</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("add-gateway")]
        public async Task<IActionResult> CreatePaymentGateway([FromForm] CreatePaymentGatewayDto bank)
        {
            if (bank is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var payment = await _service.PaymentService.AddPaymentGatewayAsync(bank);

            return Ok(payment);
        }

        /// <summary>
        /// Update payment gateways 
        /// </summary>
        /// <returns>Created payment gateway</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("update-gateway/{id:int}")]
        public async Task<IActionResult> UpdatePaymentGateway(int id, [FromForm] UpdatePaymentGatewayDto bank)
        {
            if (bank is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var payment = await _service.PaymentService.UpdatePaymentGatewayAsync(id, bank, trackChanges: true);

            return Ok(payment);
        }

        /// <summary>
        /// Gets all organisation payment gateways
        /// </summary>
        /// <returns>The payment gateways list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/gateway")]
        public async Task<IActionResult> GetAllOrganisationPaymentGateways(int organisationId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.PaymentService.GetAllOrganisationPaymentGatewaysAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.banks);
        }

        /// <summary>
        /// Gets all organisation payment gateways by id
        /// </summary>
        /// <returns>The payment gateways list</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/gateway/{id:int}")]
        public async Task<IActionResult> GetOrganisationPaymentGateway(int organisationId, int id)
        {
            var bank = await _service.PaymentService.GetOrganisationPaymentGatewayAsync(organisationId, id, trackChanges: false);

            return Ok(bank);
        }

        /// <summary>
        /// Add payment gateway to organisation
        /// </summary>
        /// <returns>The payment gateway</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/add-gateway")]
        public async Task<IActionResult> AddOrganisationPaymentGateway(int organisationId, [FromBody] IEnumerable<CreateOrganisationPaymentGatewayDto> bank)
        {
            if (bank is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var paymentGateway = await _service.PaymentService.AddPaymentGatewayToOrganisationAsync(organisationId, bank);

            return Ok(paymentGateway);
        }

        /// <summary>
        /// Update payment gateway for organisation
        /// </summary>
        /// <returns>The payment gateway</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/update-gateway/{id:int}")]
        public async Task<IActionResult> UpdateOrganisationPaymentGateway(int organisationId, int id, [FromBody] UpdateOrganisationPaymentGatewayDto bank)
        {
            if (bank is null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var paymentGateway = await _service.PaymentService.UpdatePaymentGatewayForOrganisationAsync(organisationId, id, bank, trackChanges: true);

            return Ok(paymentGateway);
        }
        /// <summary>
        /// Totals payment this current year
        /// </summary>
        /// <returns>Totals payment this current year</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/year-total-payments")]
        public async Task<IActionResult> TotalPaymentsMadeThisYear(int organisationId)
        {
            var amount = await _service.PaymentService.TotalPaymentsThisYear(organisationId);

            return Ok(amount);
        }
        /// <summary>
        /// Totals payment this current month
        /// </summary>
        /// <returns>Totals payment this current month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/monthly-total-payments")]
        public async Task<IActionResult> TotalPaymentsMadeThisMonth(int organisationId)
        {
            var amount = await _service.PaymentService.TotalPaymentsThisMonth(organisationId);

            return Ok(amount);
        }


        /// <summary>
        /// Total payments this current week
        /// </summary>
        /// <returns>Total payments this current wwek</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/weekly-total-payments")]
        public async Task<IActionResult> TotalPaymentsMadeThisWeek(int organisationId)
        {
            var amount = await _service.PaymentService.TotalPaymentsThisWeek(organisationId);

            return Ok(amount);
        }


        /// <summary>
        /// Total payments today
        /// </summary>
        /// <returns>Total payments today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/daily-total-payments")]
        public async Task<IActionResult> TotalPaymentsMadeToday(int organisationId)
        {
            var amount = await _service.PaymentService.TotalPaymentsToday(organisationId);

            return Ok(amount);
        }

        /// <summary>
        /// Total payment count for the month
        /// </summary>
        /// <returns>Total payment count for the month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/monthly-payment-count")]
        public async Task<IActionResult> TotalPaymentCountForTheMonth(int organisationId)
        {
            var count = await _service.PaymentService.TotalCountOfPaymentsThisMonth(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Total payment count for the week
        /// </summary>
        /// <returns>Total payment count for the wwek</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/weekly-payment-count")]
        public async Task<IActionResult> TotalPaymentCountForTheWeek(int organisationId)
        {
            var count = await _service.PaymentService.TotalCountOfPaymentsThisWeek(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Total payment count for today
        /// </summary>
        /// <returns>Total payment count for today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/daily-payment-count")]
        public async Task<IActionResult> DailyPaymentCount(int organisationId)
        {
            var count = await _service.PaymentService.TotalCountOfPaymentsToday(organisationId);

            return Ok(count);
        }
    }
}