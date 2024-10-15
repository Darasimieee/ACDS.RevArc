using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using System.Reflection;

namespace ACDS.RevBill.Services
{
    internal sealed class ModuleService : IModuleService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ModuleService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetModuleDto> modules, MetaData metaData)> GetAllModules(RoleParameters requestParameters, bool trackChanges)
        {
            var moduleWithMetaData = await _repository.Modules.GetAllModules(requestParameters, trackChanges);

            var moduleDto = _mapper.Map<IEnumerable<GetModuleDto>>(moduleWithMetaData);

            return (modules: moduleDto, metaData: moduleWithMetaData.MetaData);
        }

        public async Task<GetModuleDto> GetModule(int Id, bool trackChanges)
        {
            var module = await _repository.Modules.GetModule(Id, trackChanges);
            //check if the Module is null
            if (module is null)
                throw new IdNotFoundException("module", Id);

            var moduleDto = _mapper.Map<GetModuleDto>(module);

            return moduleDto;
        }
       
        public async Task<GetModuleDto> CreateModule(CreateModuleDto module)
        {
            var moduleEntity = _mapper.Map<Modules>(module);
            var modulEntity = await _repository.Modules.GetModuleName(module.ModuleName,false);
            if (modulEntity is not null)
                throw new ModuleExistsException(module.ModuleName);
            _repository.Modules.CreateModule(moduleEntity);

            await _repository.SaveAsync();

            var moduleToReturn = _mapper.Map<GetModuleDto>(moduleEntity);

            return moduleToReturn;
        }

        public async Task UpdateModule(int Id, UpdateModuleDto updateModule, bool trackChanges)
        {
            var moduleEntity = await _repository.Modules.GetModule(Id, trackChanges);
            if (moduleEntity is null)
                throw new IdNotFoundException("module", Id);

            _mapper.Map(updateModule, moduleEntity);
            await _repository.SaveAsync();
        }


    }
}
