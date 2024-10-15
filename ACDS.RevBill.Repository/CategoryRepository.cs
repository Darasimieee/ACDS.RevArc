using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Category>> GetAllCategoriesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var categories = await FindAll(trackChanges)
                .OrderBy(e => e.CategoryName)
                .ToListAsync();

            return PagedList<Category>
                .ToPagedList(categories, roleParameters.PageNumber, roleParameters.PageSize);
        }

        public async Task<PagedList<Category>> GetCategoriesbyOrgAsync(int Id,RoleParameters roleParameters, bool trackChanges)
        {
            var categories = await FindByCondition(c => c.OrganisationId.Equals(Id), trackChanges)
                .OrderBy(e => e.CategoryName)
                .ToListAsync();

            return PagedList<Category>
                .ToPagedList(categories, roleParameters.PageNumber, roleParameters.PageSize);
        }
        public async Task<Category> GetCategoryAsync(int Id, bool trackChanges) =>
            await FindByCondition(c => c.CategoryId.Equals(Id), trackChanges)
            .SingleOrDefaultAsync();
        public async Task<IEnumerable<Category>> GetCategorybyBusinessSizeAsync(int Id,int organisationId, bool trackChanges) =>
            await FindByCondition(c => c.BusinessSizeId.Equals(Id)&& c.OrganisationId.Equals(organisationId), trackChanges)
            .ToListAsync();
        public async Task<Category> GetCategorybyOrganisationId(int organisationId,string categoryName,int businessSizeId, bool trackChanges) =>
       await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.CategoryName == categoryName && c.BusinessSizeId.Equals(businessSizeId), trackChanges)
       .SingleOrDefaultAsync();
        
        public void CreateCategoryAsync(Category category) => Create(category);

     
    }   
}

