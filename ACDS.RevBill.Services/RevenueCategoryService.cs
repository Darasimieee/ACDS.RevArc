using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;

namespace ACDS.RevBill.Services
{
    internal sealed class RevenueCategoryService : IRevenueCategoryService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public RevenueCategoryService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<RevenueCategoryDto> revenueCategories, MetaData metaData)> GetAllRevenueCategoriesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var revenuesCategoryWithMetaData = await _repository.RevenueCategories.GetAllRevenueCategoriesAsync(roleParameters, trackChanges);

            var revenuesCategoryDto = _mapper.Map<IEnumerable<RevenueCategoryDto>>(revenuesCategoryWithMetaData);

            return (revenueCategories: revenuesCategoryDto, metaData: revenuesCategoryWithMetaData.MetaData);
        }

        public async Task<(IEnumerable<RevenueCategoryDto> revenueCategories, MetaData metaData)> GetOrganisationsRevenueCategoryAsync(int organisationId,RoleParameters roleParameters, bool trackChanges)
        {
            var revenueCategorysWithMetaData = await _repository.RevenueCategories.GetRevenueCategorybyOrgAsync(organisationId,roleParameters, trackChanges);

            var revenueCategoryDto = _mapper.Map<IEnumerable<RevenueCategoryDto>>(revenueCategorysWithMetaData);

            return (revenueCategories: revenueCategoryDto, metaData: revenueCategorysWithMetaData.MetaData);
        }
        public async Task<RevenueCategoryDto> GetRevenueCategoryAsync(int Id, bool trackChanges)
        {
            var revenueCategory = await _repository.RevenueCategories.GetRevenueCategoryAsync(Id, trackChanges);
            
            if (revenueCategory is null)
                throw new IdNotFoundException("Category",Id);

            var revenueCategoryDto = _mapper.Map<RevenueCategoryDto>(revenueCategory);

            return revenueCategoryDto;
        }

      
        public async Task<RevenueCategoryDto> CreateRevenueCategoryAsync(int revenueId, RevenueCategoryCreationDto revenueCategory)
        {
            var revenueCategoryEntity = _mapper.Map<RevenueCategories>(revenueCategory);
            revenueCategoryEntity.RevenueId=revenueId;
            await CheckIfOrganisationExists(revenueCategory.OrganisationId, trackChanges: false);
            await RevenueCategoryExists(revenueCategory.OrganisationId, revenueId, revenueCategory.CategoryName);
            _repository.RevenueCategories.CreateRevenueCategoryAsync(revenueCategoryEntity);
                await _repository.SaveAsync();

                var revenueCategoryToReturn = _mapper.Map<RevenueCategoryDto>(revenueCategoryEntity);
            
            return revenueCategoryToReturn;
        }
        public async Task UpdateRevenueCategoryAsync(int Id, RevenueCategoryUpdateDto revenueCategoryUpdate, bool trackChanges)
        {
            var revenueCategoryEntity = await _repository.RevenueCategories.GetRevenueCategoryAsync(Id, trackChanges);
            if (revenueCategoryEntity is null)
                throw new IdNotFoundException("Revenue Category", Id);

            _mapper.Map(revenueCategoryUpdate, revenueCategoryEntity);
            await _repository.SaveAsync();
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("Organisation",OrganisationId);
        }

        //check that revenueCategory does not exist
        private async Task RevenueCategoryExists(int organisationId,int revenueId,string revenueCategoryName)
        {
            var revenueCategoryEntity = await _repository.RevenueCategories.RevenueCategoryExists(organisationId, revenueId, revenueCategoryName, trackChanges:false);
            if (revenueCategoryEntity is not null)
                throw new NameFoundException(revenueCategoryName);

           
        }

    }
      
}

