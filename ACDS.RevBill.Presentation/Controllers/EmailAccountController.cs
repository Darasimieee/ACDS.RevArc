using System.Text.Json;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/emailAccount")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EmailAccountController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmailAccountController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all EmailAccounts
        /// </summary>
        /// <returns>The EmailAccounts list</returns>
        [HttpGet]
        public async Task<IActionResult> GetEmailAccounts([FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.EmailAccountService.GetAllEmailAccounts(roleParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.emailAccounts);
        }

        /// <summary>
        /// Gets a specific EmailAccount using their id
        /// </summary>
        /// <returns>The EmailAccount</returns>
        [HttpGet("{Id:int}", Name = "EmailAccountId")]
        public async Task<IActionResult> GetEmailAccount(int Id)
        {
            var emailAccount = await _service.EmailAccountService.GetEmailAccount(Id, trackChanges: false);

            return Ok(emailAccount);
        }

        /// <summary>
        /// Creates a new EmailAccount
        /// </summary>
        /// <returns>Successful EmailAccount creation</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmailAccount([FromBody] CreateEmailAccountDto emailAccount)
        {
            if (emailAccount is null)
                return BadRequest("CreateEmailAccountDto object is null");

            var createdEmailAccount = await _service.EmailAccountService.CreateEmailAccount(emailAccount);

            return CreatedAtRoute("EmailAccountId", new { id = createdEmailAccount.EmailAccountId }, createdEmailAccount);
        }

        /// <summary>
        /// Updates a specific EmailAccount using their id
        /// </summary>
        [HttpPost("{Id:int}")]
        public async Task<IActionResult> UpdateEmailAccount(int Id, [FromBody] UpdateEmailAccountDto emailAccount)
        {
            if (emailAccount is null)
                return BadRequest("UpdateEmailAccountDto object is null");

            await _service.EmailAccountService.UpdateEmailAccount(Id, emailAccount, trackChanges: true);

            return NoContent();
        }

        /// <summary>
        /// Gets the list of all EmailAccount's EmailTemplate by EmailAccountId
        /// </summary>
        /// <returns>The EmailAccount's EmailTemplates list</returns>
        [HttpGet("{emailAccountId:int}/emailTemplates")]
        public async Task<IActionResult> GetEmailAccountTemplates(int emailAccountId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.EmailTemplateService.GetTemplatesByAccountId(emailAccountId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.emailTemplates);
        }

        /// <summary>
        /// Gets a specific EmailTemplate using their emailTemplateId and emailAccountId
        /// </summary>
        /// <returns>The EmailTemplate</returns>
        [HttpGet("{emailAccountId:int}/{emailTemplateId:int}/emailTemplates", Name = "GetTemplatesInEmailAccount")]
        public async Task<IActionResult> GetEmailTemplate(int emailAccountId, int emailTemplateId)
        {
            var emailTemplate = await _service.EmailTemplateService.GetEmailTemplate(emailAccountId, emailTemplateId, trackChanges: false);

            return Ok(emailTemplate);
        }

        /// <summary>
        /// Creates a new EmailTemplate
        /// </summary>
        /// <returns>Successful EmailTemplate creation</returns>
        [HttpPost("{emailAccountId:int}/emailTemplates")]
        public async Task<IActionResult> CreateEmailTemplate(int emailAccountId, [FromBody] CreateEmailTemplateDto template)
        {
            if (template is null)
                return BadRequest("CreateEmailTemplateDto object is null");

            var createdtemplate = await _service.EmailTemplateService.CreateEmailTemplate(emailAccountId, template);

            return CreatedAtRoute("GetTemplatesInEmailAccount", new { emailAccountId = createdtemplate.EmailAccountId, emailTemplateId = createdtemplate.EmailTemplateId}, createdtemplate);
        }

        /// <summary>
        /// Updates a specific EmailTemplate using their id
        /// </summary>
        [HttpPost("{emailAccountId:int}/{Id:int}")]
        public async Task<IActionResult> UpdateEmailTemplate(int emailAccountId, int Id, [FromBody] UpdateEmailTemplateDto emailTemplate)
        {
            if (emailTemplate is null)
                return BadRequest("UpdateEmailTemplateDto object is null");

            await _service.EmailTemplateService.UpdateEmailTemplate(emailAccountId, Id, emailTemplate, trackChanges: true);

            return NoContent();
        }
    }
}

