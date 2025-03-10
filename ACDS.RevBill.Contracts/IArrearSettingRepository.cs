using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
    public interface IArrearSettingRepository
    {
        Task<PagedList<Arrears>> GetAllArrearSettingAsync(RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Arrears>> GetArrearbyOrgAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<Arrears> GetArrearAsync(int Id, bool trackChanges);
        void CreateArrearsAsync(Arrears createArrears);
    }
}

