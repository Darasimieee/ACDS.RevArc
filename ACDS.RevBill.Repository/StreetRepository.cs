using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ACDS.RevBill.Repository
{
    internal sealed class StreetRepository : RepositoryBase<Streets>, IStreetRepository
    {
        public StreetRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Streets>> GetAllStreetsAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var streets = await FindAll(trackChanges)
                .OrderBy(e => e.StreetName)
                .ToListAsync();

            return PagedList<Streets>
                .ToPagedList(streets, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<PagedList<Streets>> GetAllStreetsbyOrgAsync(int Id,RoleParameters roleParameters, bool trackChanges)
        {
            var street = await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
                .OrderBy(e => e.StreetName)
                .ToListAsync();

            return PagedList<Streets>
                .ToPagedList(street, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<PagedList<Streets>> GetAllStreetsbyAgencyAsync(int organisationId,int agencyId, RoleParameters roleParameters, bool trackChanges)
        {
            var street = await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.AgencyId.Equals(agencyId), trackChanges)
                .OrderBy(e => e.StreetName)
                .ToListAsync();

            return PagedList<Streets>
                .ToPagedList(street, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<Streets> GetStreetAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.StreetId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<IEnumerable<Streets>> GetStreetbyAgencyIdOrgId(int agencyId,int organisationId, bool trackChanges) =>
    await FindByCondition(c => c.AgencyId.Equals(agencyId) && c.OrganisationId.Equals(organisationId), trackChanges).OrderBy(e => e.StreetName).ToListAsync();
   
        public async Task<Streets> GetStreetbynameAsync(int agencyid, int organisationId,string  name, bool trackChanges) { 
          IQueryable<Streets> query = FindByCondition(c => c.AgencyId.Equals(agencyid) && 
                                                     c.OrganisationId == organisationId && 
                                                     c.StreetName.Equals(name), trackChanges);

    //if (wardId.HasValue)
    //{
    //    query = query.Where(c => c.WardId == wardId.Value);
    //}

    return await query.SingleOrDefaultAsync();
        }
        public void CreateStreetAsync(Streets street) => Create(street);

     
    }   
}

