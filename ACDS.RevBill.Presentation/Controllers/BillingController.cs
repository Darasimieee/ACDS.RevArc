using System.Text.Json;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/billing")]
    [ApiController]
    [Produces("application/json")]
    public class BillingController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly IQRCodeService _qrCodeService;
        public BillingController(IServiceManager service, IQRCodeService qrCodeService)
        {
            _service = service;
            _qrCodeService = qrCodeService;
        }

        /// <summary>
        /// Gets all bills in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}")]
        public async Task<IActionResult> GetAllBills(int organisationId, [FromQuery] BillingParameters requestParameters)
        {
            var pagedResult = await _service.BillingService.GetAllBillsAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.bills);
        }
        /// <summary>
        /// Gets all bills in an organisation by agency Id
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/agency/{agencyId:int}")]
        public async Task<IActionResult> GetAgencyBills(int organisationId,int agencyId , [FromQuery] BillingParameters requestParameters)
        {
            var pagedResult = await _service.BillingService.GetAgencyBillsAsync(organisationId, agencyId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.bills);
        }
        /// <summary>
        /// Gets all bills by payerId
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/agency/{agencyId:int}/payer/{payerId}")]
        public async Task<IActionResult> GetPayerIdBills(int organisationId, int agencyId,string payerId, [FromQuery] BillingParameters requestParameters)
        {
            var pagedResult = await _service.BillingService.GetPayerBillsAsync(organisationId, agencyId,payerId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.bills);
        }
        /// <summary>
        /// Gets bills using some parameter
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

        /// <summary>
        /// Gets the list of all Frequency
        /// </summary>
        /// <returns>The Frequency list</returns>
        [HttpGet("frequency")]
        public async Task<IActionResult> GetAllFrequency()
        {
            var frequencies = await _service.BillingService.GetAllFrequency(trackChanges: false);
            return Ok(frequencies);
        }

        /// <summary>
        /// Gets a single bill in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/bill/{billId:int}", Name = "GetBillById")]
        public async Task<IActionResult> GetBillById(int organisationId, int billId)
        {
            var bill = await _service.BillingService.GetBillByBillIdAsync(organisationId, billId, trackChanges: false);

            return Ok(bill);
        }

        /// <summary>
        /// Gets all bills for a customer in an organisation
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/bill/customer/{customerId:int}")]
        public async Task<IActionResult> GetBillByCustomerId(int organisationId, int customerId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.BillingService.GetBillByCustomerIdAsync(organisationId, customerId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.bills);
        }
        /// <summary>
        /// Gets all bills for a customer in an organisation with harmonisedreference
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/bill/customer/{customerId:int}/harmonized/{harmonisedbillref}")]
        public async Task<IActionResult> GetBillByHarmonisedID(int organisationId, int customerId, string harmonisedbillref)
        {
            var pagedResult = await _service.BillingService.BillByCustomerIdHarmonisedIdAsync(organisationId, customerId, harmonisedbillref);

            

            return Ok(pagedResult);
        }
        /// Gets all bills for a customer in an organisation with harmonisedreference
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/harmonized/{harmonisedbillref}/generate-bill")]
        public async Task<IActionResult> GetBillByHarmonisedID(int organisationId, string harmonisedbillref)
        {
            var pagedResult = await _service.BillingService.GenerateBillReport(organisationId, harmonisedbillref);

            

            return Ok(pagedResult);
        }
        /// <summary>
        /// Count of all bills generated this current month
        /// </summary>
        /// <returns>Count of all bills generated this current month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-bills-generated-this-month")]
        public async Task<IActionResult> TotalNoOfBillsGeneratedThisMonth(int organisationId)
        {
            var count = await _service.BillingService.NoOfBillsGeneratedThisMonth(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all bills generated this current week
        /// </summary>
        /// <returns>Count of all bills generated this current week</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-bills-generated-this-week")]
        public async Task<IActionResult> TotalNoOfBillsGeneratedThisWeek(int organisationId)
        {
            var count = await _service.BillingService.NoOfBillsGeneratedThisWeek(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Count of all bills generated today
        /// </summary>
        /// <returns>Count of all bills generated today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-of-bills-generated-today")]
        public async Task<IActionResult> TotalNoOfBillsGeneratedToday(int organisationId)
        {
            var count = await _service.BillingService.NoOfBillsGeneratedToday(organisationId);

            return Ok(count);
        }
        /// <summary>
        /// debt  report generated today
        /// </summary>
        /// <returns>unpaid bills  today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/debt-reportfor-today")]
        public async Task<IActionResult> BillstobePaidToday(int organisationId)
        {
            var result = await _service.BillingService.TotalBilltobePaidToday(organisationId);

            return Ok(result);
        }
        /// <summary>
        /// debt  report generated this Month 
        /// </summary> 
        /// <returns>unpaid bills  this month </returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/debt-reportfor-month")]
        public async Task<IActionResult> BillstobePaidThisMonth(int organisationId)
        {
            var result = await _service.BillingService.TotalBilltobePaidThisMonth(organisationId);

            return Ok(result);
        }
        /// <summary>
        /// debt  report generated this Year 
        /// </summary> 
        /// <returns>unpaid bills  this Year </returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/debt-reportthis-year")]
        public async Task<IActionResult> BillstobePaidThisYear(int organisationId)
        {
            var result = await _service.BillingService.TotalBilltobePaidThisYear(organisationId);

            return Ok(result);
        }
        /// <summary>
        /// Total amount of money generated on bills this current month
        /// </summary>
        /// <returns>Total amoout of money generated on bills this current month</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/amount-on-bills-generated-this-month")]
        public async Task<IActionResult> TotalAmountOfMoneyGeneratedOnBillsThisMonth(int organisationId)
        {
            var count = await _service.BillingService.TotalAmountOfBillsGeneratedThisMonth(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Total amount of money generated on bills this current week
        /// </summary>
        /// <returns>Total amoout of money generated on bills this current week</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/amount-on-bills-generated-this-week")]
        public async Task<IActionResult> TotalAmountOfMoneyGeneratedOnBillsThisWeek(int organisationId)
        {
            var count = await _service.BillingService.TotalAmountOfBillsGeneratedThisWeek(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Total amount of money generated on bills today
        /// </summary>
        /// <returns>Total amoout of money generated on bills today</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/amount-on-bills-generated-today")]
        public async Task<IActionResult> TotalAmountOfMoneyGeneratedOnBillsToday(int organisationId)
        {
            var count = await _service.BillingService.TotalAmountOfBillsGeneratedToday(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Generate bill for property after enumeration
        /// </summary>
        /// <returns>Generated Bill</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/generate-bill/property/{propertyId:int}/customer/{customerId:int}")]
        public async Task<IActionResult> GenerateBillForPropertyAfterEnumeration(int organisationId, int propertyId, int customerId, [FromBody] CreateBulkPropertyBill createBill)
        {
            if (createBill.CreatePropertyBillDto.Count==0)
                return BadRequest("CreateBillDto object is null");
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var generateBill = await _service.BillingService.CreatePropertyBillAsync(organisationId, propertyId, customerId, createBill, trackChanges: false);

           
            return Ok(generateBill);
        }

        /// <summary>                                                                                                                                                                                             
        /// Generate bill for non-property 
        /// </summary>
        /// <returns>Generated Bill</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/generate-bill/customer/{customerId:int}")]
        public async Task<IActionResult> GenerateBillForNonProperty(int organisationId, int customerId, [FromBody] CreateBulkNonProperty createBill)
        {
            if (createBill is null)
                return BadRequest("CreateBillDto object is null");
             
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var generateBill = await _service.BillingService.CreateNonPropertyBillAsync(organisationId, customerId, createBill, trackChanges: false);

            return Ok(new
            {
                message = ("Bill generated sucessfully")
                
        });
            
        }
       
        /// <summary>
        /// Generate backlog bill for property 
        /// </summary>
        /// <returns>Generated Bill</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/generate-backlog-bill/property/{propertyId:int}/customer/{customerId:int}")]
        public async Task<IActionResult> GenerateBacklogBill(int organisationId, int propertyId, int customerId, [FromBody] CreateBulkBacklogBill createBill)
        {
            if (createBill is null)
                return BadRequest("CreateBillDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var generateBill = await _service.BillingService.BackLogBill(organisationId, propertyId, customerId, createBill, trackChanges: false);

            return Ok(new
            {
                message = ("Bill generated sucessfully")
            });
        }

        /// <summary>
        /// Bill bulk upload
        /// </summary>
        /// <returns>Generated Bills</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/bill-bulk-upload")]
        public async Task<IActionResult> BulkBilling(int organisationId, CreateBulkBillingDto bulkBilling)
        {
            if (bulkBilling is null)
                return BadRequest("CreateBillDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var bill = await _service.BillingService.BulkBilling(organisationId, bulkBilling, trackChanges: false);

            return Ok(new
            {
                message = ("Bill generated sucessfully")
            });
        }
       
        /// <summary>
        /// Auto Bill-Generation 
        /// </summary>
        /// <returns>Generated Bills</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/auto-billing")]
        public async Task<IActionResult> AutoBilling(int organisationId, CreateAutoBill createBillDto)
        {
            if (createBillDto is null)
                return BadRequest("CreateBillDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var bill = await _service.BillingService.AutoBillGeneration(organisationId, createBillDto, trackChanges: false);

            return Ok(new
            {
                message = ("Bill generated sucessfully")
            });
        }

        /// <summary>
        /// Validate bill reference
        /// </summary>
        /// <returns>Status of bill reference</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("validate-bill")]
        public async Task<IActionResult> ValidateBillReference(ValidateBillRequest1Dto validateBill)
        {
            if (validateBill is null)
                return BadRequest("validateBill object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var validation =  await _service.BillingService.ValidateBill(validateBill);

            return Ok(validation);
        }

        /// <summary>
        /// Validate harmonized bill reference
        /// </summary>
        /// <returns>Status of bill reference</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("validate-harmonized-bill")]
        public IActionResult ValidateHarmonizedBillReference(HarmonizedBillReferenceRequestDto validateBill)
        {
            if (validateBill is null)
                return BadRequest("validateBill object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var validation = _service.BillingService.ValidateHarmonizedBillReferences(validateBill);

            return Ok(validation);
        }

        /// <summary>
        /// Count and amount of all generated bills by agency
        /// </summary>
        /// <returns>Count and amount of all generated bills by agency</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/count-and-amount-of-bills-by-area-office")]
        public async Task<IActionResult> CountAndAmountByAreaOffice(int organisationId)
        {
            var count = await _service.BillingService.NoOfBillsByAreaOffice(organisationId);

            return Ok(count);
        }

        /// <summary>
        /// Create Bill Format
        /// </summary>
        /// <returns>Generated Bill Format</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/create-bill-format")]
        public async Task<IActionResult> CreateBillFormat(int organisationId, [FromForm] CreateBillFormat createBillFormat)
        {
            var bill = await _service.BillingService.CreateBillFormatAsync(organisationId, createBillFormat, trackChanges: false);

            return Ok(bill);
        }

        /// <summary>
        /// Update Bill Format
        /// </summary>
        /// <returns>Generated Bill Format</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/bill-format/{billFormatId:int}/update-bill-format")]
        public async Task<IActionResult> UpdateBillFormat(int organisationId, int billFormatId, [FromForm] UpdateBillFormat updateBillFormat)
        {
            var bill = await _service.BillingService.UpdateBillFormatAsync(organisationId, billFormatId, updateBillFormat, trackChanges: true);

            return Ok(bill);
        }

        /// <summary>
        /// Update Bill 
        /// </summary>
        /// <returns>Update Bill </returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/property/{propertyId:int}/customer/{customerId:int}/update-bill")]
        public async Task<IActionResult> UpdateBill(int organisationId,int propertyId, int customerId, [FromBody] UpdatePropertyBill updateBill)
        {
            if (updateBill is null)
                return BadRequest("UpdatePropertyBillDto  object is null");
            //if (year != DateTime.Now.Year.ToString())
            //    return BadRequest("you can only upgrade this year's bill");
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var bill = await _service.BillingService.UpdateBillAsync(organisationId, propertyId, customerId, updateBill, true);

            return Ok(bill);
        }
        /// <summary>
        /// Get All Bill Formats
        /// </summary>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/bill-format")]
        public async Task<IActionResult> GetAllBillFormats(int organisationId, [FromQuery] DefaultParameters requestParameters)
        {
            var pagedResult = await _service.BillingService.GetAllBillFormatsAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.bills);
        }

        /// <summary>
        /// Get Bill Format
        /// </summary>
        /// <returns>Generated Bill Format</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/bill-format/{billFormatId:int}")]
        public async Task<IActionResult> GetBillFormat(int organisationId, int billFormatId)
        {
            var bill = await _service.BillingService.GetBillFormatAsync(organisationId, billFormatId, trackChanges: false);

            return Ok(bill);
        }


        /// <summary>
        /// Generate Bill for Template
        /// </summary>
        /// <returns>Generated Bill Format</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{organisationId:int}/bill/{billId:long}/generate-bill")]
        public async Task<IActionResult> GenerateBillReport(int organisationId, long billId)    
        {
            var bill = await _service.BillingService.GenerateBillReport(organisationId, billId);

            return Ok(bill);
        }

        /// <summary>
        /// Generate Bill for Template
        /// </summary>
        /// <returns>Generated Bill Format</returns>
        /// <response code = "200" > Successful </ response >
        /// < response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/upload-bill-preview")]
        public async Task<IActionResult> UploadBillPreview(int organisationId, [FromQuery] DefaultParameters requestParameters, CancellationToken ct)
        {
            if (Request.Form.Files.Count == 0)
                return BadRequest("No content!");

            var file = Request.Form.Files[0];
            var creator = Request.Form["createdby"];
            var bills = await _service.BillingService.UploadBillsAsync(organisationId, creator, requestParameters, file);
      
            return Ok(bills);
        }
        /// <summary>
        /// Create Bills for previewed uploads
        /// </summary>
        /// <returns>Generated Bills and ungenerated bills</returns>
        /// <response code="200">Successful</response>
        /// <response code="500">Internal Server Error</response> 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{organisationId:int}/{createdby}/create-bills-previewed")]
        public async Task<IActionResult> BulkPreviwedBilling(int organisationId,string createdby ,IEnumerable<CreatePropertyBillUpload> bulkBilling)
        {
            if (bulkBilling is null)
                return BadRequest("CreateBillDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var bill = await _service.BillingService.BulkPreviewedBilling(organisationId, createdby, bulkBilling, trackChanges: false);

            return Ok(bill);
        }


    }
}