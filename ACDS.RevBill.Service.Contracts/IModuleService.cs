using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IModuleService
    {
        Task<(IEnumerable<GetModuleDto> modules, MetaData metaData)> GetAllModules(RoleParameters roleParameters, bool trackChanges);
        Task<GetModuleDto> GetModule(int Id, bool trackChanges);
        Task<GetModuleDto> CreateModule(CreateModuleDto module);
        Task UpdateModule(int Id, UpdateModuleDto updateModule, bool trackChanges);
    }
}
