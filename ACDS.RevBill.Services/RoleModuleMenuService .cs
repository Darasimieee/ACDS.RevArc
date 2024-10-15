using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using System.Reflection;

namespace ACDS.RevBill.Services
{
    internal sealed class RoleModuleMenuService : IRoleModuleMenuService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private DataContext _context;

        public RoleModuleMenuService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<(IEnumerable<GetRoleModMenuDto> RoleModules, MetaData metaData)> GetAllRoleModuleMenusAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges)
        {
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            var WithRoleModuleMetaData = await _repository.RoleModuleMenus.GetAllRoleModuleMenusAsync(organisationId, roleId, requestParameters, trackChanges);

            var organisationRoleModuleDto = _mapper.Map<IEnumerable<GetRoleModMenuDto>>(WithRoleModuleMetaData);

            return (RoleModules: organisationRoleModuleDto, metaData: WithRoleModuleMetaData.MetaData);
        }

        public async Task<GetRoleModMenuDto> GetRoleModuleMenuAsync(int organisationId, int roleModuleMenuId, bool trackChanges)
        {
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);

            var roleModule = await _repository.RoleModuleMenus.GetRoleModuleMenuAsync(organisationId, roleModuleMenuId, trackChanges);
            //check if the organisation is null
            if (roleModule is null)
                throw new IdNotFoundException("role module menu with RoleModuleMenuId", roleModuleMenuId);

            var roleModuleDto = _mapper.Map<GetRoleModMenuDto>(roleModule);

            return roleModuleDto;
        }

        public async Task<Response> CreateRoleModuleMenuAsync(int organisationId, int roleId, List<CreateRoleModMenusDto> RoleModule)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            foreach (CreateRoleModMenusDto  i in RoleModule)
            {
                 foreach(var j in i.MenusDto) { 
                    foreach(var k in j.MenuIds) { 
                var roleModule = await _repository.RoleModuleMenus.CheckModuleMenuInRoleAsync(organisationId, roleId, j.ModuleId, k, false);

                                if (roleModule is  null)
                                {
                                    CreateRoleModMenuDto createRoleModMenuDto = new CreateRoleModMenuDto
                                    {
                                        RoleId = roleId,
                                        ModuleId=j.ModuleId,
                                        MenuId =k,
                                        DateCreated = DateTime.Now, 
                                        CreatedBy = i.CreatedBy 

                                    }; 
                                             var RoleModuleEntity = _mapper.Map<RoleModuleMenus>(createRoleModMenuDto);
                                           RoleModuleEntity.RoleId = roleId;
                                           RoleModuleEntity.OrganisationId = organisationId;   
                                            _repository.RoleModuleMenus.CreateRoleModuleMenu(RoleModuleEntity);
                                            await _repository.SaveAsync();
                                }
                        
                    }
                 }
            }

            dataResponse.StatusMessage = "Menu successfully added to role";
            dataResponse.Status = 200;

            return dataResponse;
        }           

        public async Task<Response> UpdateRoleModuleMenusAsync(int organisationId, int roleId, List<UpdateRoleModMenusDto> updateRoleModuleMenu, bool trackChanges)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            var roleModuleMenuEntity = await _repository.RoleModuleMenus.GetRoleModuleMenuAsync(organisationId, roleId, trackChanges);
            //if (roleModuleMenuEntity is null)
            //{
            //    CreateRoleModMenuDto createRoleModMenuDto = new CreateRoleModMenuDto
            //    {
            //        RoleId = roleId,
            //        ModuleId = j.ModuleId,
            //        MenuId = k,
            //        DateCreated = DateTime.Now,
            //        CreatedBy = i.CreatedBy

            //    };
            //    var RoleModuleEntity = _mapper.Map<RoleModuleMenus>(createRoleModMenuDto);
            //    RoleModuleEntity.RoleId = roleId;
            //    RoleModuleEntity.OrganisationId = organisationId;
            //    _repository.RoleModuleMenus.CreateRoleModuleMenu(RoleModuleEntity);
            //    await _repository.SaveAsync();
            //}

            _mapper.Map(updateRoleModuleMenu, roleModuleMenuEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Role Module Menu is Sucessfully Update";
            dataResponse.Status = 200;

            return dataResponse;
        }

    }
}