using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Repository
{
    internal sealed class MenusRepository : RepositoryBase<Menus>, IMenusRepository
    {
        public MenusRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<PagedList<Menus>> GetAllMenus(int moduleId,RoleParameters requestParameters, bool trackChanges)
        {
            var menus = 
                await FindByCondition(c => c.ModuleId.Equals(moduleId), trackChanges)
                .OrderBy(e => e.MenuName)
                .ToListAsync();

            return PagedList<Menus>
                .ToPagedList(menus, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Menus> GetMenu(int moduleId,int Id, bool trackChanges) =>
            await FindByCondition(c => c.MenuId.Equals(Id) && c.ModuleId== moduleId, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<Menus> GetMenubyName(int moduleId, string Name, bool trackChanges) =>
           await FindByCondition(c => c.MenuName.Equals(Name) && c.ModuleId == moduleId, trackChanges)
           .SingleOrDefaultAsync();

        public void CreateMenu(Menus menus) => Create(menus);
    }
}
