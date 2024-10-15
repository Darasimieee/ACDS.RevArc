using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;

namespace ACDS.RevBill.Services
{
    internal sealed class MenuService : IMenuService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public MenuService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<GetMenuDto> menus, MetaData metaData)> GetAllMenus(int moduleId,RoleParameters requestParameters, bool trackChanges)
        {
            var menuWithMetaData = await _repository.Menus.GetAllMenus(moduleId,requestParameters, trackChanges);

            var menuDto = _mapper.Map<IEnumerable<GetMenuDto>>(menuWithMetaData);

            return (menus: menuDto, metaData: menuWithMetaData.MetaData);
        }

        public async Task<GetMenuDto> GetMenu(int moduleId,int menuId, bool trackChanges)
        {
            var menu = await _repository.Menus.GetMenu(moduleId,menuId, trackChanges);
            //check if the menu is null
            if (menu is null)
                throw new IdNotFoundException("menu", menuId);

            var menuDto = _mapper.Map<GetMenuDto>(menu);

            return menuDto;
        }

        public async Task<GetMenuDto> CreateMenu(int moduleId, CreateMenuDto createMenuDto)
        {
            var menuEntity = _mapper.Map<Menus>(createMenuDto);
            menuEntity.ModuleId = moduleId;
            var menuModel = await _repository.Menus.GetMenubyName(moduleId, createMenuDto.MenuName, false);
            if (menuModel is not null)
                throw new ModuleExistsException(createMenuDto.MenuName);
            _repository.Menus.CreateMenu(menuEntity);
            await _repository.SaveAsync();

            var menuToReturn = _mapper.Map<GetMenuDto>(menuEntity);

            return menuToReturn;
        }

        public async Task UpdateMenu(int moduleId, int Id, UpdateMenuDto updateMenu, bool trackChanges)
        {
            var menuEntity = await _repository.Menus.GetMenu(moduleId,Id, trackChanges);
            if (menuEntity is null)
                throw new IdNotFoundException("menu", Id);

            _mapper.Map(updateMenu, menuEntity);
            await _repository.SaveAsync();
        }
    }
}
