using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{ }
//    internal sealed class WardRepository : RepositoryBase<Ward>, IWardRepository
//    {
//        public WardRepository(RepositoryContext repositoryContext)
//        : base(repositoryContext)
//        {
//        }

//        public async Task<IEnumerable<Ward>> GetAllWardsAsync(int organisationId, bool trackChanges) =>
//           await FindByCondition(c => c.OrganisationId.Equals(organisationId), trackChanges)
//               .OrderBy(e => e.Id)
//               .ToListAsync();

//        public async Task<Ward> GetWardAsync(int organisationId, int wardId, bool trackChanges) =>
//            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.Id.Equals(wardId), trackChanges)
//            .SingleOrDefaultAsync();

//        public void CreateWard(int organisationId, Ward ward)
//        {
//            ward.OrganisationId = organisationId;
//            Create(ward);
//        }
//    }
//}