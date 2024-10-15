using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Agencies;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using Response = ACDS.RevBill.Entities.Response;

namespace ACDS.RevBill.Services
{
    internal sealed class StreetService :  IStreetService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly PID _pidConfig;
        private DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StreetService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context, IWebHostEnvironment webHostEnvironment, PID pidConfig)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _pidConfig = pidConfig;
            _context= context;  
        }

        public async Task<(IEnumerable<GetStreetDto> street, MetaData metaData)> GetAllStreetsAsync(RoleParameters roleParameters, bool trackChanges)
    {
            var streetsWithMetaData = await _repository.Streets.GetAllStreetsAsync(roleParameters, trackChanges);

            var streetsDto = _mapper.Map<IEnumerable<GetStreetDto>>(streetsWithMetaData);

            return (streets: streetsDto, metaData: streetsWithMetaData.MetaData);
        }

        public async Task<(IEnumerable<GetStreetDto> street, MetaData metaData)> GetOrganisationsStreetAsync(int organisationId, RoleParameters roleParameters, bool trackChanges)
      {
            var streetsWithMetaData = await _repository.Streets.GetAllStreetsbyOrgAsync(organisationId,roleParameters, trackChanges);

            var streetsDto = _mapper.Map<IEnumerable<GetStreetDto>>(streetsWithMetaData);

            return (streets: streetsDto, metaData: streetsWithMetaData.MetaData);
        }
        public async Task<(IEnumerable<GetStreetDto> street, MetaData metaData)> GetAgencyStreetAsync(int organisationId,int agencyId, RoleParameters roleParameters, bool trackChanges)
        {
            var streetsWithMetaData = await _repository.Streets.GetAllStreetsbyAgencyAsync(organisationId, agencyId, roleParameters, trackChanges);

            var streetsDto = _mapper.Map<IEnumerable<GetStreetDto>>(streetsWithMetaData);

            return (streets: streetsDto, metaData: streetsWithMetaData.MetaData);
        }
        public async Task<GetStreetDto> GetStreetAsync(int Id, bool trackChanges)
        {
            var street = await _repository.Streets.GetStreetAsync(Id, trackChanges);
            //check if the Street is null
            if (street is null)
                throw new IdNotFoundException("Street",Id);

            var streetDto = _mapper.Map<GetStreetDto>(street);

            return streetDto;
        }
        public async Task<IEnumerable<GetStreetDto>> GetStreetbyAgencyIdOrgId(int agencyId, int organisationId, bool trackChanges)
        {
            var street = await _repository.Streets.GetStreetbyAgencyIdOrgId(agencyId, organisationId, trackChanges);
            //check if the Agency is null
            if (street is null)
                throw new IdNotFoundException("Street", agencyId);
            var streetDto = _mapper.Map<IEnumerable<GetStreetDto>>(street);

            return streetDto;
        }

        public async Task<Response> UploadStreetAsync(int organisationId, int agencyId,string creator, IFormFile file)
        {
            Response response= new Response();

            var excelImporter = new ExcelHelper();

            var filePath = SaveFile(file);
            List<UploadCreationDto> notcreated = new List<UploadCreationDto>();

            var streetsRequests = excelImporter.ImportExcel<UploadCreationDto>(filePath);
            foreach (var street in streetsRequests)
            {
                //int wardId = 0;
                //if (!street.Ward.IsNullOrEmpty()) {
                //    wardId = _context.Wards.Where(x => x.WardName == street.Ward && x.OrganisationId== organisationId).FirstOrDefault().Id;

                //}
                response = await StreetExists(organisationId, agencyId, street.StreetName);


                if (response.Data == null)
                {
                    var streetEntity = new Streets();
                    streetEntity.Active = true;
                    streetEntity.AgencyId = agencyId;                     
                    streetEntity.DateCreated = DateTime.Now;
                    streetEntity.CreatedBy = creator;
                    streetEntity.OrganisationId = organisationId;
                   // streetEntity.WardId = wardId;
                    streetEntity.StreetName = street.StreetName;
                    streetEntity.Description = street.Description;

        _repository.Streets.CreateStreetAsync(streetEntity);
                    await _repository.SaveAsync();
                 

                }
                else{

                notcreated.Add(street);
                
                }
            }
           if(notcreated.Count > 0)
            {
                response.StatusMessage = "Below item(s) already exists ";
                response.Data = notcreated;
                response.Status = 200;
            }
            else { 
            response.StatusMessage = "Successfully uploaded";
            response.Status = 200;
            }
            return response;
        }
        private string SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles");
            }
            var folderPath = Path.Combine(webRootPath, "Uploadstreet");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            //gets all files in source directory & delete previous ones except the last 23
            foreach (var fileItem in new DirectoryInfo(folderPath).GetFiles())
            {
                DateTime dt = File.GetCreationTime(fileItem.FullName);

                if (dt < DateTime.Today)
                {
                  File.Delete(fileItem.FullName);
                }
            }
            var fileslist = Directory.GetFiles(folderPath);
            file.CopyTo(stream);

            return filePath;
        }
        public async Task<Response> CreateStreetAsync(BulkStreetCreation street)
        {
            var streetEntity = _mapper.Map<IEnumerable<Streets>>(street.StreetCreation);

            Response response = new Response();
            foreach(Streets item in streetEntity) {

            response= await CheckIfOrganisationExists(item.OrganisationId,trackChanges:false);

            response=await StreetExists( item.OrganisationId, item.AgencyId, item.StreetName);

            if (response.Data == null) { 
                _repository.Streets.CreateStreetAsync(item);
                await _repository.SaveAsync();

                var streetToReturn = _mapper.Map<IEnumerable<GetStreetDto>>(streetEntity);
            response.Status = 200;

            response.StatusMessage = "Street successfully created!";

            response.Data = streetToReturn;
            }
            }
            return response;
        }
        public async Task<Response> UpdateStreetAsync(int Id, StreetUpdateDto streetUpdate, bool trackChanges)
        {
            var streetEntity = await _repository.Streets.GetStreetAsync(Id, trackChanges);
            Response response = new Response();
            if (streetEntity is null)
            {
                response.Status = 200;
                response.StatusMessage = "The street " + Id + " does not exists";
                return response;
            }

            _mapper.Map(streetUpdate, streetEntity);
            await _repository.SaveAsync();
            response.Status = 200;
            response.StatusMessage = "Successfully Updated!";

            return response;    
        }
       
        private async Task<Response> CheckIfOrganisationExists(int OrganisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(OrganisationId, trackChanges);
            Response response = new Response();
            if (company is null)
            {
                response.Status = 200;
                response.StatusMessage = "The organisation " + OrganisationId + " does not exists";

            }


            return response;
        }

        //check existence of Agency 
        private async Task<Entities.Response> StreetExists(int organisationId, int agencyId,  string streetName)
        {
            var streetEntity = await _repository.Streets.GetStreetbynameAsync(agencyId, organisationId, streetName, trackChanges:false);
            Response response = new Response();
            if (streetEntity is not null) {
                response.Status = 200;
                response.Data = streetEntity;
                response.StatusMessage = "The Street " + streetName + " already exists";
            }

            return response;
            
        }

    }
      
}

