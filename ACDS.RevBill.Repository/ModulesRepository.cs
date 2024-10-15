using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Repository
{
    internal sealed class ModulesRepository : RepositoryBase<Modules>, IModulesRepository
    {
        public ModulesRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public async Task<PagedList<Modules>> GetAllModules(RoleParameters requestParameters, bool trackChanges)
        {
            var modules = await FindAll(trackChanges)
                .OrderBy(e => e.ModuleName)
                .ToListAsync();

            return PagedList<Modules>
                .ToPagedList(modules, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Modules> GetModule(int Id, bool trackChanges) =>
            await FindByCondition(c => c.ModuleId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<Modules> GetModuleName(String modulename, bool trackChanges) =>
           await FindByCondition(c => c.ModuleName.Equals(modulename), trackChanges)
           .SingleOrDefaultAsync();
        public void CreateModule(Modules modules) => Create(modules);

        // public void DeleteLga(Lgas lgas) => Delete(lgas);

    }
  
}
