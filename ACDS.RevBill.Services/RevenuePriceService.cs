using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;

namespace ACDS.RevBill.Services
{
    internal sealed class RevenuePriceService : IRevenuePriceService
    {
        private readonly IRepositoryManager _repository;
        private DataContext _context;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public RevenuePriceService(IRepositoryManager repository, ILoggerManager logger, DataContext context, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<(IEnumerable<RevenuePricesDto> revenuePrices, MetaData metaData)> GetAllRevenuePricesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var revenuePricesWithMetaData = await _repository.RevenuePrices.GetAllRevenuePricesAsync(roleParameters, trackChanges);

            var revenuePricesDto = _mapper.Map<IEnumerable<RevenuePricesDto>>(revenuePricesWithMetaData);

            return (revenuePrices: revenuePricesDto, metaData: revenuePricesWithMetaData.MetaData);
        }

        public async Task<(IEnumerable<RevenuePriceListDto> revenuePriceList, MetaData metaData)> GetOrganisationsRevenuePricesAsync(int organisationId,RoleParameters roleParameters, bool trackChanges)
        {
            var revenuePricesWithMetaData = await _repository.RevenuePrices.GetRevenuePricesbyOrgAsync(organisationId, trackChanges);

        var revenuePricesDto = _mapper.Map<IEnumerable<RevenuePricesDto>>(revenuePricesWithMetaData);

            List<RevenuePriceListDto> priceListDto = new List<RevenuePriceListDto>();

            foreach(var item in revenuePricesDto)
            {
                var revenueLPrice = _mapper.Map<RevenuePriceListDto>(item);
             var businessSize= _context.BusinessSizes
                    .Join(
                            _context.Category,
                            b => b.Id, 
                            c => c.BusinessSizeId,
                            (b, c) => new
                            {
                                BusinessSizeName = b.BusinessSizeName,
                                CategoryId = c.CategoryId,  
           
                          }).Where(x=>x.CategoryId== revenueLPrice.CategoryId).SingleOrDefault();
                if(businessSize== null)
                {
                    //throw new Exception($"business size not found for category: {revenueLPrice.CategoryId}");
                    revenueLPrice.BusinessSize = "N/A";
                }
                else
                {
                    revenueLPrice.BusinessSize = businessSize.BusinessSizeName;
                }
                                         
                priceListDto.Add(revenueLPrice);
            }
            var revenuePriceLists=PagedList<RevenuePriceListDto>
                .ToPagedList(priceListDto, roleParameters.PageNumber, roleParameters.PageSize);

            return (revenuePrices: priceListDto, metaData: revenuePriceLists.MetaData);
        }
        public async Task<(IEnumerable<RevenuePricesDto> revenuePrices, MetaData metaData)> GetCategorysRevenuePricesAsync(int organisationId, int categoryId, RoleParameters roleParameters, bool trackChanges)
        {
            var revenuePricesWithMetaData = await _repository.RevenuePrices.GetRevenuePricesbyCatAsync(organisationId, categoryId, roleParameters, trackChanges);

            var revenuePricesDto = _mapper.Map<IEnumerable<RevenuePricesDto>>(revenuePricesWithMetaData);

            return (revenuePrices: revenuePricesDto, metaData: revenuePricesWithMetaData.MetaData);
        }
        public async Task<RevenuePricesDto> GetRevenuePriceAsync(int Id, bool trackChanges)
        {
            var revenuePrices = await _repository.RevenuePrices.GetRevenuePriceAsync(Id, trackChanges);
            
            if (revenuePrices is null)
                throw new IdNotFoundException("Revenue price", Id);

            var revenuePricesDto = _mapper.Map<RevenuePricesDto>(revenuePrices);

            return revenuePricesDto;
        }

      
        public async Task<RevenuePricesDto> CreateRevenuePriceAsync(int revenueId, RevenuePricesCreationDto revenuePrices)
        {
            var revenuePricesEntity = _mapper.Map<RevenuePrices>(revenuePrices);
            revenuePricesEntity.RevenueId = revenueId;
            await CheckIfOrganisationExists(revenuePrices.OrganisationId, trackChanges: false);
            await RevenuePricesExists(revenuePrices.OrganisationId, revenuePrices.CategoryName, revenuePricesEntity.RevenueId,revenuePrices.Amount,trackChanges:false);
            _repository.RevenuePrices.CreateRevenuePriceAsync(revenuePricesEntity);
                await _repository.SaveAsync();

            //var revenue = await _repository.Revenues.GetRevenueByIdAsync(revenueId); 
            //if (revenue == null)
            //{
            //    throw new KeyNotFoundException("Revenue not found."); 
            //}

            //var revenueDescription = revenue.Description;

            var revenuePricesToReturn = _mapper.Map<RevenuePricesDto>(revenuePricesEntity);
            
            return revenuePricesToReturn;
        }
        public async Task<IEnumerable<RevenuePricesDto>> GetRevenuePriceByRevenueAsync(int RevenueId, bool trackChanges)
        {
            var revenuePrices = await _repository.RevenuePrices.GetRevenuePriceByRevenueAsync(RevenueId,  trackChanges);

            var revenuePricesDto = _mapper.Map<IEnumerable<RevenuePricesDto>>(revenuePrices);

            return revenuePricesDto;
        }

        public async Task UpdateRevenuePriceAsync(int Id, RevenuePricesUpdateDto revenuePricesUpdate, bool trackChanges)
        {
            var revenuePricesEntity = await _repository.RevenuePrices.GetRevenuePriceAsync(Id, trackChanges);
            if (revenuePricesEntity is null)
                throw new IdNotFoundException("Revenue price", Id);

            _mapper.Map(revenuePricesUpdate, revenuePricesEntity);
            await _repository.SaveAsync();
        }
       
        private async Task CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("Organisation", OrganisationId);
        }

        //check that revenueprice does not exist
        private async Task RevenuePricesExists(int organisationId,string categoryName, int revenueId, decimal amount, bool trackChanges)
        {
            var revenuePricesEntity = await _repository.RevenuePrices.RevenuePriceExists(organisationId, categoryName, revenueId, amount, trackChanges);
            if (revenuePricesEntity is not null)
                throw new NameFoundException(amount.ToString());

           
        }

    }
      
}

