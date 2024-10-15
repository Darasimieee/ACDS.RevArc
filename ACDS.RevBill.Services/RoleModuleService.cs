using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;

namespace ACDS.RevBill.Services
{
    internal sealed class RoleModuleService : IRoleModuleservice
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private DataContext _context;

        public RoleModuleService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<(IEnumerable<GetRoleModuleDto> RoleModules, MetaData metaData)> GetAllRoleModulesAsync(int organisationId, int roleId, RoleParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var WithRoleModuleMetaData = await _repository.RoleModules.GetAllRoleModulesAsync(organisationId, roleId, requestParameters, trackChanges);

            var organisationRoleModuleDto = _mapper.Map<IEnumerable<GetRoleModuleDto>>(WithRoleModuleMetaData);

            return (RoleModules: organisationRoleModuleDto, metaData: WithRoleModuleMetaData.MetaData);
        }

        public async Task<GetRoleModuleDto> GetRoleModuleAsync(int organisationId, int roleId, int moduleId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var roleModule = await _repository.RoleModules.GetRoleModuleAsync(organisationId, roleId, moduleId, trackChanges);
            //check if the organisation is null
            if (roleModule is null)
                throw new IdNotFoundException("role module with ModuleId", moduleId);

            var roleModuleDto = _mapper.Map<GetRoleModuleDto>(roleModule);

            return roleModuleDto;
        }

        public async Task<Response> CreateRoleModuleAsync(int organisationId, int roleId, List<CreateRoleModuleDto> RoleModule)
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

            foreach (CreateRoleModuleDto i in RoleModule)
            {
                var RoleModuleEntity = _mapper.Map<RoleModules>(i);
               
                RoleModuleEntity.RoleId = roleId;
                var roleModule = await _repository.RoleModules.CheckModuleInRoleAsync(organisationId, roleId, RoleModuleEntity.ModuleId, false);

                if (roleModule is not null)
                {
                    throw new RoleModuleExistsException(RoleModuleEntity.ModuleId);
                }
                _repository.RoleModules.CreateRoleModule(organisationId, RoleModuleEntity);
                await _repository.SaveAsync();
            }

            dataResponse.StatusMessage = "Modules successfully added to role";
            dataResponse.Status = 200;

            return dataResponse;
        }           

        public async Task<Response> UpdateRoleModuleAsync(int organisationId, int roleId, int moduleId, UpdateRoleModuleDto updateRoleModuleOrganisation, bool trackChanges)
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

            var organisationRoleModuleEntity = await _repository.RoleModules.GetRoleModuleAsync(organisationId, roleId, moduleId, trackChanges);
            if (organisationRoleModuleEntity is null)
                throw new IdNotFoundException("Role Module with ModuleId", moduleId);

            _mapper.Map(updateRoleModuleOrganisation, organisationRoleModuleEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Role Module Sucessfully Update";
            dataResponse.Status = 200;

            return dataResponse;
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int organisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(organisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", organisationId);
        }
    }
}