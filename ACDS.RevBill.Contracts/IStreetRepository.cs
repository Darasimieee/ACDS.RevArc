using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace ACDS.RevBill.Contracts
{
    public interface IStreetRepository
    {
        Task<PagedList<Streets>> GetAllStreetsAsync(RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Streets>> GetAllStreetsbyOrgAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<PagedList<Streets>> GetAllStreetsbyAgencyAsync(int organisationId, int agencyId, RoleParameters roleParameters, bool trackChanges);    
        Task<Streets> GetStreetbynameAsync(int agencyid,  int organisationId, string name, bool trackChanges);
        Task<Streets> GetStreetAsync(int Id, bool trackChanges);
        Task<IEnumerable<Streets>> GetStreetbyAgencyIdOrgId(int agencyId, int organisationId, bool trackChanges);
        void CreateStreetAsync(Streets street);
    }
}

