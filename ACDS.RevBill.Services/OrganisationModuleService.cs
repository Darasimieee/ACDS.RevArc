using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Services
{
    internal sealed class OrganisationModuleService : IOrganisationModuleService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly DataContext _context; 
        private readonly IMapper _mapper;
      
        public OrganisationModuleService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;           
        }

        public async Task<(IEnumerable<GetOrganisationModuleDto> organisationModules, MetaData metaData)> GetAllOrganisationModulesAsync(int organisationId, RoleParameters requestParameters, bool trackChanges)
        {
            var organistaionWithModuleMetaData = await _repository.OrganisationModules.GetAllOrganisationModulesAsync(organisationId, requestParameters, trackChanges);

            var organisationModuleDto = _mapper.Map<IEnumerable<GetOrganisationModuleDto>>(organistaionWithModuleMetaData );

            return (organisationModules: organisationModuleDto, metaData: organistaionWithModuleMetaData.MetaData);
        }

        public async Task<GetOrganisationModuleDto> GetOrganisationModuleAsync(int moduleId, int organisationId, bool trackChanges)
        {
            var organisationModule = await _repository.OrganisationModules.GetOrganisationModuleAsync(moduleId,organisationId, trackChanges);
            //check if the organisation is null
            if (organisationModule is null)
                throw new IdNotFoundException("organisation module", organisationId);

            var organisationModuleDto = _mapper.Map<GetOrganisationModuleDto>(organisationModule);

            return organisationModuleDto;
        }

        public async Task<Response> CreateOrganisationModuleAsync(int organisationId,List<CreateOrganisationModuleDto> organisationModule)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId,false);
            var orgModule =await _context.OrganisationModules.Where(x => x.OrganisationId== organisationId).ToListAsync();

            if (orgModule is not null)
            {
                foreach (var item in orgModule)
                {                                
                    item.Status = ((int)Status.Inactive);
                    item.DateModified = DateTime.UtcNow;
                    item.ModifiedBy = "System";
                    
                   _context.OrganisationModules.Update(item);
                   _context.SaveChanges(); 
                }
            }

            foreach (CreateOrganisationModuleDto i  in organisationModule)
            {                
                var entityModule = orgModule.Where(x => x.ModuleId == i.ModuleId).FirstOrDefault();
                if (entityModule is not null)
                {
                    entityModule.Status = i.Status;
                    _context.OrganisationModules.Update(entityModule);
                    _context.SaveChanges();
                }
                else
                {
                    var moduleEntity = _mapper.Map<OrganisationModules>(i);
                    moduleEntity.OrganisationId = organisationId;
                    moduleEntity.Status = ((int)Status.Active);

                    _repository.OrganisationModules.CreateOrganisationModule(moduleEntity);
                } 

                await _repository.SaveAsync();
            }

            dataResponse.StatusMessage = "Module added successfully";
            dataResponse.Status = 200;

            return dataResponse;
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", OrganisationId);
        }
    }
}