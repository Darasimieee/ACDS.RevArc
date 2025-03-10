using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class ArrearSettingRepository : RepositoryBase<Arrears>, IArrearSettingRepository
    {

        public ArrearSettingRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {

        }

        public async Task<PagedList<Arrears>> GetAllArrearSettingAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var arrears = await FindAll(trackChanges)
                .OrderBy(e => e.ArrearId)
                .ToListAsync();

            return PagedList<Arrears>
                .ToPagedList(arrears, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<PagedList<Arrears>> GetArrearbyOrgAsync(int organisationId, RoleParameters roleParameters, bool trackChanges)
        {
            var arrears = await FindByCondition(c => c.OrganisationId.Equals(organisationId), trackChanges)
                .OrderByDescending(e => e.CreatedBy)
                .ToListAsync();

            return PagedList<Arrears>
                .ToPagedList(arrears, roleParameters.PageNumber, roleParameters.PageSize);

        }
        public async Task<Arrears> GetArrearAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.ArrearId.Equals(Id), trackChanges)
            .Include(o => o.organisation)
            .SingleOrDefaultAsync();


        public void CreateArrearsAsync(Arrears createArrears) => Create(createArrears);



    }
}

