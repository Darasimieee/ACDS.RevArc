using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IMenuService
    {
        Task<(IEnumerable<GetMenuDto> menus, MetaData metaData)> GetAllMenus(int moduleId,RoleParameters roleParameters, bool trackChanges);
        Task<GetMenuDto> GetMenu(int moduleId, int menuId, bool trackChanges);
        Task<GetMenuDto> CreateMenu(int moduleId, CreateMenuDto createMenuDto);
        Task UpdateMenu(int moduleId, int Id, UpdateMenuDto updateMenu, bool trackChanges);
    }
}
