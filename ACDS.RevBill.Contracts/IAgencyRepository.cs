using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Contracts
{
    public interface IAgencyRepository
    {
        Task<PagedList<Agencies>> GetAllAgenciesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Agencies>> GetAllAgenciesbyOrgAsync(int Id, RoleParameters roleParameters, bool trackChanges);
        Task<Agencies> GetAgencybynameAsync(int organisationId, string agencyName, bool trackChanges);
        Task<Agencies> GetAgencyAsync(int Id, bool trackChanges);
        Task<Agencies> GetAgencybyIdOrgId(int Id, int organisationId, bool trackChanges);        
        void CreateAgencyAsync(Agencies agency);
    }
}

