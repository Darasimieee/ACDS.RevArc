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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;

namespace ACDS.RevBill.Services
{
    internal sealed class RevenueService : IRevenueService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly PID _pidConfig;
        private DataContext _context;
        public JsonModelService _modelService;
        private readonly IMapper _mapper;
        public RevenueService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context, JsonModelService modelService, PID pidConfig)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _pidConfig = pidConfig;
            _context = context;
            _modelService = modelService;
        }

        public async Task<(IEnumerable<RevenueDto> revenues, MetaData metaData)> GetAllRevenuesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var revenuesWithMetaData = await _repository.Revenues.GetAllRevenuesAsync(roleParameters, trackChanges);

            var revenuesDto = _mapper.Map<IEnumerable<RevenueDto>>(revenuesWithMetaData);

            return (agencies: revenuesDto, metaData: revenuesWithMetaData.MetaData);
        }
        public async Task<IEnumerable<HeadRevenue>> GetRevenuesbyHeadAsync(int organisationId,  bool trackChanges)
        {
            var revenuesWithMetaData = await _repository.HeadRevenues.GetRevenuesbyHeadAsync(organisationId, trackChanges);
           

            return (revenuesWithMetaData);
        }

        public async Task<Response> CreateheadRevenuesAsync(int organisationId, bool trackChanges)
        {
            Response response= new Response();
            int revenueExist = 0;
            var organisation = await _context.Organisations.Where(x => x.OrganisationId.Equals(organisationId)).FirstOrDefaultAsync();
            //get all revenues under organisation
            var dbrevenues = await _context.HeadRevenue.Where(x => x.OrganisationId.Equals(organisationId)).ToListAsync();
       
            var agencyhead = organisation.AgencyCode.Substring(0, 3);
            //Add all revenue under agency head
            var revenues = _modelService.GetEBSRevenueByCode(agencyhead);
            foreach (var item in revenues.DistinctBy(x=>x.RevCode))
            {
               
                if (dbrevenues.Count > 0) { 
                 revenueExist = dbrevenues.Where(x => x.RevenueCode.Equals(item.RevCode)).SingleOrDefault().HeadRevenueId;
                }
                if (revenueExist==null || revenueExist==0) {
                HeadRevenue revenue = new HeadRevenue
                {
                    
                    AgencyHead = agencyhead,
                    OrganisationId = organisationId,
                    RevenueCode = item.RevCode,
                    RevenueName = item.RevName,
                    DateCreated = DateTime.Now,
                    CreatedBy = "System"
                };
                _context.HeadRevenue.AddAsync(revenue);    
                    _context.SaveChanges();
                }
               
            }
            
            
            response.Status = 200;
            response.StatusMessage = "Successfully refreshed";
            
            return (response);
        }
        public async Task<(IEnumerable<RevenueDto> revenues, MetaData metaData)> GetOrganisationsRevenueAsync(int organisationId,RoleParameters roleParameters, bool trackChanges)
        {
            var revenuesWithMetaData = await _repository.Revenues.GetRevenuesbyOrgAsync(organisationId,roleParameters, trackChanges);

            var revenuesDto = _mapper.Map<IEnumerable<RevenueDto>>(revenuesWithMetaData);

            //revenuesDto.Where(x=>x.BusinessTypeId==)

            return (agencies: revenuesDto, metaData: revenuesWithMetaData.MetaData);
        }
        public async Task<(IEnumerable<RevenueDto> revenues, MetaData metaData)> GetRevenuesbyBusinessTypeAsync(int organisationId, int businessTypeId, RoleParameters roleParameters, bool trackChanges)
        {
            var revenuesWithMetaData = await _repository.Revenues.GetRevenuesbyBusinessTypeAsync(organisationId, businessTypeId, roleParameters, trackChanges);

            var revenuesDto = _mapper.Map<IEnumerable<RevenueDto>>(revenuesWithMetaData);

            return (agencies: revenuesDto, metaData: revenuesWithMetaData.MetaData);
        }
        public async Task<RevenueDto> GetRevenueAsync(int Id, bool trackChanges)
        {
            var revenue = await _repository.Revenues.GetRevenueAsync(Id, trackChanges);
            
            if (revenue is null)
                throw new IdNotFoundException("Revenue", Id);

            var revenueDto = _mapper.Map<RevenueDto>(revenue);

            return revenueDto;
        }

      
        public async Task<Response> CreateRevenueAsync(RevenueCreationDto revenue)
        {
            Response response = new();
            var revenueEntity = _mapper.Map<Revenues>(revenue);
            await CheckIfOrganisationExists(revenue.OrganisationId, trackChanges: false);
              var revenuexist = await RevenueAndDescriptionExists(revenue.OrganisationId, revenue.RevenueName,revenue.BusinessTypeId);
            if (revenuexist is not null)
            {
                response.StatusMessage = new NameFoundException(revenue.RevenueName).Message;
                response.Status = 400;
                return response;
            }
            _repository.Revenues.CreateRevenueAsync(revenueEntity);
                await _repository.SaveAsync();
            response.Status = 200;
            response.Data = _mapper.Map<RevenueDto>(revenueEntity);
            response.StatusMessage = "Revenue successfully created";
            return response;
        }
        public async Task UpdateRevenueAsync(int Id, RevenueUpdateDto revenueUpdate, bool trackChanges)
        {
            var revenueEntity = await _repository.Revenues.GetRevenueAsync(Id, trackChanges);
            if (revenueEntity is null)
                throw new IdNotFoundException("Revenue", Id);

            _mapper.Map(revenueUpdate, revenueEntity);
            await _repository.SaveAsync();
        }
        //private async Task<List<AgencyResponseDto>> GetEbsAgencies()
        //{
        //    var agencyEntity = new AgencyRequestDto();

        //    var agencyUrl = _pidConfig.BASE_URL+ _configuration.GetSection("Agency").Value;
        //    var agencyHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + _pidConfig.STATE);

        //    //map hash, key and state
        //    agencyEntity.Hash = agencyHash;
        //    agencyEntity.State = _pidConfig.STATE;
        //    agencyEntity.ClientId = _pidConfig.CLIENT_ID;

        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Post, agencyUrl);

        //    //serialize payload 
        //    var payload = JsonConvert.SerializeObject(agencyEntity);
        //    var content = new StringContent(payload, null, "application/json");
        //    request.Content = content;

        //    //get response
        //    var response = await client.SendAsync(request);
        //    response.EnsureSuccessStatusCode();
        //    var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

        //    //deserialise response
        //    var deserialisedResponseContent = JsonConvert.DeserializeObject(responseContent);

        //    var agencies = _mapper.Map<List<AgencyResponseDto>>(deserialisedResponseContent);

        //    return agencies;
        //}
        //helper methods
        private async Task CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("Organisation", OrganisationId);
        }

        //check that revenue does not exist
        private async Task<Revenues> RevenueExists(int organisationId,string revenueName)
        {
           return await _repository.Revenues.GetRevenuebyOrganisationId(organisationId, revenueName, trackChanges:false);
             
        }

        private async Task<Revenues> RevenueAndDescriptionExists(int organisationId, string revenueName, int businessTypeId)
        {
            return await _repository.Revenues.GetRevenuebyOrganisationIdAndDescription(organisationId, revenueName, businessTypeId, trackChanges: false);

        }

    }
      
}

