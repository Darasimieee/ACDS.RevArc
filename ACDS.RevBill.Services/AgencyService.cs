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
    internal sealed class AgencyService : IAgencyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly PID _pidConfig;
        private DataContext _context;
        public JsonModelService _modelService;
        private readonly IMapper _mapper;
        public AgencyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context, JsonModelService modelService, PID pidConfig)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _pidConfig = pidConfig;
            _context = context;
            _modelService = modelService;
        }

        public async Task<(IEnumerable<AgencyDto> agencies, MetaData metaData)> GetAllAgenciesAsync(RoleParameters roleParameters, bool trackChanges)
        {
            var agenciesWithMetaData = await _repository.Agencies.GetAllAgenciesAsync(roleParameters, trackChanges);

            var agenciesDto = _mapper.Map<IEnumerable<AgencyDto>>(agenciesWithMetaData);

            return (agencies: agenciesDto, metaData: agenciesWithMetaData.MetaData);
        }

        public async Task<(IEnumerable<AgencyDto> agencies, MetaData metaData)> GetOrganisationsAgencyAsync(int organisationId,RoleParameters roleParameters, bool trackChanges)
        {
            var agenciesWithMetaData = await _repository.Agencies.GetAllAgenciesbyOrgAsync(organisationId,roleParameters, trackChanges);

            var agenciesDto = _mapper.Map<IEnumerable<AgencyDto>>(agenciesWithMetaData);

            return (agencies: agenciesDto, metaData: agenciesWithMetaData.MetaData);
        }
        public async Task<AgencyDto> GetAgencyAsync(int Id, bool trackChanges)
        {
            var agency = await _repository.Agencies.GetAgencyAsync(Id, trackChanges);
            //check if the Agency is null
            if (agency is null)
                throw new IdNotFoundException("Agency",Id);

            var agencyDto = _mapper.Map<AgencyDto>(agency);

            return agencyDto;
        }
        public async Task<AgencyDto> GetAgencybyIdOrgId(int Id, int organisationId,bool trackChanges)
        {
            var agency = await _repository.Agencies.GetAgencybyIdOrgId(Id, organisationId, trackChanges);
            //check if the Agency is null
            if (agency is null)
                throw new IdNotFoundException("Agency", Id);
            var agencyDto = _mapper.Map<AgencyDto>(agency);

            return agencyDto;
        }

        public async Task<Response> FetchAgencyAsync(int organisationId)
        {
            Response response = new Response();
            string head = "";
            string tenantId = "";
            var dbagencies = await _context.Agencies.Where(x => x.OrganisationId.Equals(organisationId)).ToListAsync();
            if(dbagencies.Count==0)
            {
                var organisation = await _context.Organisations.Where(x => x.OrganisationId.Equals(organisationId)).FirstOrDefaultAsync();
                //var userprofile= await _context.UserProfiles.Where(x => x.OrganisationId.Equals(organisationId)).FirstOrDefaultAsync();

                head = organisation.AgencyCode.Substring(0, 3);
                tenantId = organisation.TenantName;
            }
            else
            {
                head = dbagencies.First().AgencyCode.Substring(0, 3);
                tenantId = dbagencies.First().TenantName;
            }
            //get all agencies under organisation
            

            var agencies = await _modelService.GetEBSAgencyByCode(head);
            foreach (var item in agencies.DistinctBy(x => x.AgencyRef))
            {
               var exists= dbagencies.Where(x => x.AgencyCode.Equals(item.AgencyRef)).Count();
                if (exists == 0) { 
                bool agencytype = false;
                if (item.AgencyRef.Substring(3, 4) == "0000")
                {
                    agencytype = true;
                }
                Agencies agenc = new Agencies
                {
                    OrganisationId = organisationId,
                    AgencyCode = item.AgencyRef,
                    AgencyName = item.FullName,
                    Description = item.FullName,
                    IsHead = agencytype,
                    Active = true,
                    DateCreated = DateTime.Now,
                    CreatedBy = "System",
                    TenantName = tenantId
                };
                _context.Agencies.Add(agenc);
                _context.SaveChanges();
                }
            }
            response.StatusMessage = "Successfully refreshed";
            response.Status = 200;
            return response;
        }
        public async Task<AgencyDto> CreateAgencyAsync(AgencyCreationDto agency)
        {
            var agencyEntity = _mapper.Map<Agencies>(agency);
            await CheckIfOrganisationExists(agency.OrganisationId,trackChanges:false);
            await AgencyExists(agency.OrganisationId, agency.AgencyName);
                _repository.Agencies.CreateAgencyAsync(agencyEntity);
                await _repository.SaveAsync();

                var agencyToReturn = _mapper.Map<AgencyDto>(agencyEntity);
            
            return agencyToReturn;
        }
        public async Task UpdateAgencyAsync(int Id, AgencyUpdateDto agencyUpdate, bool trackChanges)
        {
            var agencyEntity = await _repository.Agencies.GetAgencyAsync(Id, trackChanges);
            if (agencyEntity is null)
                throw new IdNotFoundException("Agency",Id);

            _mapper.Map(agencyUpdate, agencyEntity);
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

        //check existence of Agency 
        private async Task AgencyExists(int organisationId,string agencyName)
        {
            var agencyEntity = await _repository.Agencies.GetAgencybynameAsync(organisationId, agencyName, trackChanges:false);
            if (agencyEntity is not null)
            throw new NameFoundException(agencyName);
            
        }

    }
      
}

