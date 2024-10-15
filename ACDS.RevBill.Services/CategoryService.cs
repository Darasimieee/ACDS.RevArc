using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;

namespace ACDS.RevBill.Services
{
    internal sealed class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CategoryService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<CategoryDto> categories, MetaData metaData)> GetAllCategoriesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var categoriesWithMetaData = await _repository.Category.GetAllCategoriesAsync(roleParameters, trackChanges);

            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categoriesWithMetaData);

            return (categories: categoryDto, metaData: categoriesWithMetaData.MetaData);
        }

        public async Task<(IEnumerable<CategoryDto> categories, MetaData metaData)> GetOrganisationsCategoryAsync(int organisationId,RoleParameters roleParameters, bool trackChanges)
        {
            var categoriesWithMetaData = await _repository.Category.GetCategoriesbyOrgAsync(organisationId,roleParameters, trackChanges);

            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(categoriesWithMetaData);

            return (categories: categoryDto, metaData: categoriesWithMetaData.MetaData);
        }
        public async Task<Response> GetCategoryAsync(int Id, bool trackChanges)
        {
            Response response = new Response();
             var category = await _repository.Category.GetCategoryAsync(Id, trackChanges);
            
            if (category is null)
                throw new IdNotFoundException("Category", Id);

            var categoryDto = _mapper.Map<CategoryDto>(category);
            response.Status = 200;
            response.Data = categoryDto;    
            response.StatusMessage = "OK";
            return response;
        }
        public async Task<IEnumerable<CategoryDto>> GetCategorybyBusinessSize(int businessSizeId,int organisationId, bool trackChanges)
         {
            var category = await _repository.Category.GetCategorybyBusinessSizeAsync(businessSizeId, organisationId, trackChanges);

            if (category is null)
                throw new IdNotFoundException("Category", businessSizeId);

            var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(category);

            return categoryDto;
        }


        public async Task<Response> CreateCategoryAsync(CategoryCreationDto category)
        {
            Response dataResponse = new Response();
            var categoryEntity = _mapper.Map<Category>(category);
            await CheckIfOrganisationExists(category.OrganisationId, trackChanges: false);
            var categoryexist= await CategoryExists(category.OrganisationId, category.CategoryName,category.BusinessSizeId);
            if (categoryexist is not null) { 
                dataResponse.StatusMessage = new NameFoundException(category.CategoryName).Message;
               dataResponse.Status = 400;
                return dataResponse;
            }
            
            _repository.Category.CreateCategoryAsync(categoryEntity);
                await _repository.SaveAsync();
            dataResponse.StatusMessage = "Category successfully created";
            dataResponse.Status = 200;
            dataResponse.Data = _mapper.Map<CategoryDto>(categoryEntity);
            


            return dataResponse;
         
        }
        public async Task UpdateCategoryAsync(int Id, CategoryUpdateDto categoryUpdate, bool trackChanges)
        {
            var categoryEntity = await _repository.Category.GetCategoryAsync(Id, trackChanges);
            if (categoryEntity is null)
                throw new IdNotFoundException("Category", Id);

            _mapper.Map(categoryUpdate, categoryEntity);
            await _repository.SaveAsync();
        }
        
        private async Task CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("Organisation",OrganisationId);
        }

        //check that Category does not exist
        public async Task<Category> CategoryExists(int organisationId,string categoryName,int businessSizeId)
        {
            
            return await _repository.Category.GetCategorybyOrganisationId(organisationId, categoryName, businessSizeId, trackChanges:false);
            
           

           
        }

    }
      
}

