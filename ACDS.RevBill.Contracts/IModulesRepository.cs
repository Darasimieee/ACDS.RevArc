using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
	public interface IModulesRepository
	{
        Task<PagedList<Modules>> GetAllModules(RoleParameters roleParameters, bool trackChanges);
        Task<Modules> GetModuleName(String modulename, bool trackChanges);
        Task<Modules> GetModule(int ModuleId, bool trackChanges);
        void CreateModule(Modules modules);
    }
}

