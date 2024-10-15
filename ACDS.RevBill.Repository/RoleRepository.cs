using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class RoleRepository : RepositoryBase<Roles>, IRoleRepository
    {
        public RoleRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Roles>> GetAllRolesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var roles = await FindAll(trackChanges)
                .OrderBy(e => e.RoleName)
                .ToListAsync();

            return PagedList<Roles>
                .ToPagedList(roles, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<Roles> GetRoleAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.RoleId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateRole(Roles roles) => Create(roles);

        public void DeleteRole(Roles roles) => Delete(roles);
    }   
}

