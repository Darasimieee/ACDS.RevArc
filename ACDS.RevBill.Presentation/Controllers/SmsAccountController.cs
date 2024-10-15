using System;
using System.ComponentModel.Design;
using System.Text.Json;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.SmsAccount;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACDS.RevBill.Presentation.Controllers
{
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/smsAccount")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class SmsAccountController : ControllerBase
    {
        private readonly IServiceManager _service;

        public SmsAccountController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all SmsAccounts
        /// </summary>
        /// <returns>The SmsAccounts list</returns>
        [HttpGet]
        public async Task<IActionResult> GetSmsAccounts([FromQuery] RoleParameters roleParameters)
        {
            var pagedResult = await _service.SmsAccountService.GetAllSmsAccounts(roleParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.smsAccounts);
        }

        /// <summary>
        /// Gets a specific SmsAccount using their id
        /// </summary>
        /// <returns>The SmsAccount</returns>
        [HttpGet("{Id:int}", Name = "smsAccountId")]
        public async Task<IActionResult> GetSmsAccount(int Id)
        {
            var smsAccount = await _service.SmsAccountService.GetSmsAccount(Id, trackChanges: false);

            return Ok(smsAccount);
        }

        /// <summary>
        /// Creates a new SmsAccount
        /// </summary>
        /// <returns>Successful SmsAccount creation</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSmsAccount([FromBody] CreateSmsAccountDto smsAccount)
        {
            if (smsAccount is null)
                return BadRequest("CreateEmailAccountDto object is null");

            var createdSmsAccount = await _service.SmsAccountService.CreateSmsAccount(smsAccount);

            return CreatedAtRoute("SmsAccountId", new { id = createdSmsAccount.SmsAccountId }, createdSmsAccount);
        }

        /// <summary>
        /// Updates a specific SmsAccount using their id
        /// </summary>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateSmsAccount(int Id, [FromBody] UpdateSmsAccountDto smsAccount)
        {
            if (smsAccount is null)
                return BadRequest("UpdateSmsAccountDto object is null");

            await _service.SmsAccountService.UpdateSmsAccount(Id, smsAccount, trackChanges: true);

            return NoContent();
        }
        /// <summary>
        /// Gets the list of all SmsAccount's SmsTemplate by SmsAccountId
        /// </summary>
        /// <returns>The SmsAccount's SmsTemplates list</returns>
        [HttpGet("{smsAccountId:int}/smsTemplates")]
        public async Task<IActionResult> GetSmsAccountTemplates(int smsAccountId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.SmsTemplateService.GetTemplatesBySmsAccountId(smsAccountId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.SmsTemplates);
        }
        /// <summary>
        /// Gets a specific SmsTemplate using their smsTemplateId and smsAccountId
        /// </summary>
        /// <returns>The SmsTemplate</returns>
        [HttpGet("{smsAccountId:int}/{smsTemplateId:int}/smsTemplates", Name = "GetTemplatesInSmsAccount")]
        public async Task<IActionResult> GetSmsTemplate(int smsAccountId, int smsTemplateId)
        {
            var smsTemplate = await _service.SmsTemplateService.GetSmsTemplate(smsAccountId, smsTemplateId, trackChanges: false);

            return Ok(smsTemplate);
        }
        /// <summary>
        /// Creates a new SmsTemplate
        /// </summary>
        /// <returns>Successful SmsTemplate creation</returns>
        [HttpPost("{smsAccountId:int}/smsTemplates")]
        public async Task<IActionResult> CreateSmsTemplate(int smsAccountId, [FromBody] CreateSmsTemplateDto template)
        {
            if (template is null)
                return BadRequest("CreateSmsTemplateDto object is null");

            var createdtemplate = await _service.SmsTemplateService.CreateSmsTemplate(smsAccountId, template);

            return CreatedAtRoute("GetTemplatesInSmsAccount", new { smsAccountId = createdtemplate.SmsAccountId, smsTemplateId = createdtemplate.SmsTemplateId }, createdtemplate);
        }
        /// <summary>
        /// Updates a specific SmsTemplate using their id
        /// </summary>
        [HttpPut("{smsAccountId:int}/{Id:int}")]
        public async Task<IActionResult> UpdateSmsTemplate(int smsAccountId, int Id, [FromBody] UpdateSmsTemplateDto smsTemplate)
        {
            if (smsTemplate is null)
                return BadRequest("UpdateSmsTemplateDto object is null");

            await _service.SmsTemplateService.UpdateSmsTemplate(smsAccountId, Id, smsTemplate, trackChanges: true);

            return NoContent();
        }


    }
}

