using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;


namespace ACDS.RevBill.Contracts
{
     public interface IMenusRepository
     {
        Task<PagedList<Menus>> GetAllMenus(int moduleId, RoleParameters roleParameters, bool trackChanges);
        Task<Menus> GetMenu(int moduleId, int MenuId, bool trackChanges);
        Task<Menus> GetMenubyName(int moduleId, string Name, bool trackChanges);
        void CreateMenu(Menus menus);    
     }
}
