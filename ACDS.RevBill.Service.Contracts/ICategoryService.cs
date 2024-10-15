using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Responses;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface ICategoryService
    {
        Task<(IEnumerable<CategoryDto> categories, MetaData metaData)> GetAllCategoriesAsync(RoleParameters roleParameters, bool trackChanges);
        Task<(IEnumerable<CategoryDto> categories, MetaData metaData)> GetOrganisationsCategoryAsync(int organisationId, RoleParameters roleParameters, bool trackChanges);
        Task<Response> GetCategoryAsync(int Id, bool trackChanges);
        Task<IEnumerable<CategoryDto>> GetCategorybyBusinessSize(int businessSizeId,int organisationId, bool trackChanges);
        Task<Response> CreateCategoryAsync(CategoryCreationDto category);
        Task UpdateCategoryAsync(int Id, CategoryUpdateDto categoryUpdate, bool trackChanges);
    }
}

