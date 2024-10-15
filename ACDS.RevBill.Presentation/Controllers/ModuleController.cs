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
    [Authorize]
    [AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/module")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ModuleController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ModuleController(IServiceManager service) => _service = service;

        /// <summary>
        /// Gets the list of all Modules
        /// </summary>
        /// <returns>The Modules list</returns>
        [HttpGet]
        public async Task<IActionResult> GetModules([FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.ModuleService.GetAllModules(requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.modules);
        }

        /// <summary>
        /// Gets a specific Module using their id
        /// </summary>
        /// <returns>The Module</returns>
        [HttpGet("{id:int}", Name = "ModuleById")]
        public async Task<IActionResult> GetModule(int id)
        {
            var module = await _service.ModuleService.GetModule(id, trackChanges: false);

            return Ok(module);
        }

        /// <summary>
        /// Creates a new Module
        /// </summary>
        /// <returns>Successful Module creation</returns>
        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto module)
        {
            if (module is null)
                return BadRequest("CreateModuleDto object is null");

            var createdModule = await _service.ModuleService.CreateModule(module);

            return CreatedAtRoute("ModuleById", new { id = createdModule.ModuleId }, createdModule);
        }

        /// <summary>
        /// Updates a specific Module using their id
        /// </summary>
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] UpdateModuleDto module)
        {
            if (module is null)
                return BadRequest("UpdateModuleDto object is null");

            await _service.ModuleService.UpdateModule(id, module, trackChanges: true);

            return Ok("Module successfully updated");
        }

        /// <summary>
        /// Gets the list of all Module menus by moduleId
        /// </summary>
        /// <returns>The Module's Menu list</returns>
        [HttpGet("{moduleId:int}/menus")]
        public async Task<IActionResult> GetModuleMenus(int moduleId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.MenuService.GetAllMenus(moduleId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.menus);
        }

        /// <summary>
        /// Gets a specific Menu using their id and ModuleId
        /// </summary>
        /// <returns>The Menu</returns>
        [HttpGet("{moduleId:int}/menus/{id:int}", Name = "GetMenuInModule")]
        public async Task<IActionResult> GetMenu(int moduleId, int id)
        {
            var menu = await _service.MenuService.GetMenu(moduleId, id, trackChanges: false);

            return Ok(menu);
        }

        /// <summary>
        /// Creates a new Menu
        /// </summary>
        /// <returns>Successful Menu creation</returns>
        [HttpPost("{moduleId:int}/menus")]
        public async Task<IActionResult> CreateModuleMenu(int moduleId, [FromBody] CreateMenuDto menu)
        {
            if (menu is null)
                return BadRequest("CreateMenuDto object is null");

            var createdMenu = await _service.MenuService.CreateMenu(moduleId,menu);

            return Ok(createdMenu);
        }

        /// <summary>
        /// Updates a specific Menu using their id
        /// </summary>
        [HttpPost("{moduleId:int}/menus/{id:int}")]
        public async Task<IActionResult> UpdateMenu(int moduleId, int id, [FromBody] UpdateMenuDto menu)
        {
            if (menu is null)
                return BadRequest("UpdateMenuDto object is null");

            await _service.MenuService.UpdateMenu(moduleId, id, menu, trackChanges: true);

            return NoContent();
        }

        /// <summary>
        /// Gets the list of all Module in an organisation by OrganisationId
        /// </summary>
        /// <returns>The Organisation's Module list</returns>
        [HttpGet("{organisationId:int}/organisation-modules")]
        public async Task<IActionResult> GetOrganisationModules(int organisationId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.OrganisationModuleService.GetAllOrganisationModulesAsync(organisationId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.organisationModules);
        }

        /// <summary>
        /// Gets a specific Module using their id and OrganisationId
        /// </summary>
        /// <returns>The Organisation</returns>
        [HttpGet("{moduleId:int}/organisation-modules/{organisationModuleId:int}", Name = "GetModuleforOrganisation")]
        public async Task<IActionResult> GetOrganisationModule(int moduleId, int organisationId)
        {
            var menu = await _service.OrganisationModuleService.GetOrganisationModuleAsync(moduleId, organisationId, trackChanges: false);
             
            return Ok(menu);
        }

        /// <summary>
        /// Creates a new organisationmodule
        /// </summary>
        /// <returns>Successful organisationmodule creation</returns>
        /// <response code="200">Successful</response>
        /// 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("{organisationId:int}/organisation-modules")]
        public async Task<IActionResult> CreateOrganisationModule(int organisationId, [FromBody]List<CreateOrganisationModuleDto> organisationmodule)
        {
            if (organisationmodule is null)
                return BadRequest("CreateOrganisationModuleDto object is null");

            var module = await _service.OrganisationModuleService.CreateOrganisationModuleAsync(organisationId, organisationmodule);

            return Ok(module);
        }
        
        /// <summary>
        /// Gets the list of all modules in a role by moduleId
        /// </summary>
        /// <returns>The Role's Module list</returns>
        [HttpGet("{organisationId:int}/role/{roleId:int}/role-modules")]
        public async Task<IActionResult> GetRoleModules(int organisationId, int roleId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RoleModuleservice.GetAllRoleModulesAsync(organisationId, roleId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.RoleModules);
        }

        /// <summary>
        /// Gets a specific RoleModule using their id and RoleModuleId
        /// </summary>
        /// <returns>The RoleModule</returns>
        [HttpGet("{organisationId:int}/role/{roleId:int}/role-modules/{id:int}", Name = "GetModuleforRole")]
        public async Task<IActionResult> GetRoleModule(int organisationId, int roleId, int id)
        {
            var menu = await _service.RoleModuleservice.GetRoleModuleAsync(organisationId, roleId, id, trackChanges: false);

            return Ok(menu);
        }

        /// <summary>
        /// Creates a new rolemodule 
        /// </summary>
        /// <returns>Successful rolemodule creation</returns>
        [HttpPost("{organisationId:int}/role/{roleId:int}/role-modules")]
        public async Task<IActionResult> CreateOrganisationRoleModule(int organisationId, int roleId, [FromBody]List<CreateRoleModuleDto> rolemodule)
        {
            if (rolemodule is null)
                return BadRequest("CreateRoleModuleDto object is null");

            var module = await _service.RoleModuleservice.CreateRoleModuleAsync(organisationId, roleId, rolemodule);

            return Ok(module);
        }

        /// <summary>
        /// Updates a specific rolemodule using their id
        /// </summary>
        [HttpPost("{organisationId:int}/role/{roleId:int}/role-modules/{id:int}")]
        public async Task<IActionResult> UpdateRolemodule(int organisationId, int roleId, int id, [FromBody] UpdateRoleModuleDto rolemodule)
        {
            if (rolemodule is null)
                return BadRequest("UpdateRoleModuleDto object is null");

            var module = await _service.RoleModuleservice.UpdateRoleModuleAsync(organisationId, roleId, id, rolemodule, trackChanges: true);

            return Ok(module);
        }
        /// <summary>
        /// Gets the list of all Role module menus in a role by RoleId
        /// </summary>
        /// <returns>The Role's Module list</returns>
        [HttpGet("{organisationId:int}/role/{roleId:int}/role-module-menus")]
        public async Task<IActionResult> GetRoleModuleMenus(int organisationId, int roleId, [FromQuery] RoleParameters requestParameters)
        {
            var pagedResult = await _service.RoleModuleMenuService.GetAllRoleModuleMenusAsync(organisationId, roleId, requestParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.RoleModules);
        }

        /// <summary>
        /// Gets a specific Role Module menu using organisation id and RoleModuleId
        /// </summary>
        /// <returns>The RoleModule</returns>
        [HttpGet("{organisationId:int}/role-module-menu/{roleModuleId:int}")]
        public async Task<IActionResult> GetRoleModuleMenu(int organisationId,  int roleModuleId)
        {
            var menu = await _service.RoleModuleMenuService.GetRoleModuleMenuAsync(organisationId,  roleModuleId, trackChanges: false);

            return Ok(menu);
        }

        /// <summary>
        /// Creates a new list of rolemodulemenu
        /// </summary>
        /// <returns>Successful rolemodule creation</returns>
        [HttpPost("{organisationId:int}/role/{roleId:int}/role-module-menu")]
        public async Task<IActionResult> CreateRoleModuleMenu(int organisationId, int roleId, [FromBody]List<CreateRoleModMenusDto> rolemodulemenu)
        {
            if (rolemodulemenu is null)
                return BadRequest("CreateRoleModuleDto object is null");

            var module = await _service.RoleModuleMenuService.CreateRoleModuleMenuAsync(organisationId, roleId, rolemodulemenu);

            return Ok(module);
        }

        /// <summary>
        /// Updates a specific role module menu  using the organization id and  roleid
        /// </summary>
        [HttpPost("{organisationId:int}/role-module-menu-Update/role/{roleId:int}")]
        public async Task<IActionResult> UpdateRolemodulemenu(int organisationId, int roleId, [FromBody] List<UpdateRoleModMenusDto> rolemodule)
        {
            if (rolemodule is null)
                return BadRequest("UpdateRoleModuleDto object is null");

            var module = await _service.RoleModuleMenuService.UpdateRoleModuleMenusAsync(organisationId, roleId, rolemodule,true);

            return Ok(module);
        }
    }
}