using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;

namespace ACDS.RevBill.Services
{
    internal sealed class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public RoleService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<RoleDto> roles, MetaData metaData)> GetAllRolesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var rolesWithMetaData = await _repository.Roles.GetAllRolesAsync(roleParameters, trackChanges);

            var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(rolesWithMetaData);

            return (roles: rolesDto, metaData: rolesWithMetaData.MetaData);
        }

        public async Task<RoleDto> GetRoleAsync(int Id, bool trackChanges)
        {
            var role = await _repository.Roles.GetRoleAsync(Id, trackChanges);
            //check if the role is null
            if (role is null)
                throw new IdNotFoundException("role", Id);

            var roleDto = _mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleForCreationDto role)
        {
            var roleEntity = _mapper.Map<Roles>(role);

            _repository.Roles.CreateRole(roleEntity);
            await _repository.SaveAsync();

            var roleToReturn = _mapper.Map<RoleDto>(roleEntity);

            return roleToReturn;
        }

        public async Task UpdateRoleAsync(int Id, RoleForUpdateDto roleForUpdate, bool trackChanges)
        {
            var roleEntity = await _repository.Roles.GetRoleAsync(Id, trackChanges);
            if (roleEntity is null)
                throw new IdNotFoundException("role", Id);

            _mapper.Map(roleForUpdate, roleEntity);
            await _repository.SaveAsync();
        }

        public async Task DeleteRoleAsync(int Id, bool trackChanges)
        {
            var roleEntity = await _repository.Roles.GetRoleAsync(Id, trackChanges);
            if (roleEntity is null)
                throw new IdNotFoundException("role", Id);

            _repository.Roles.DeleteRole(roleEntity);
            await _repository.SaveAsync();
        }
    }
}

