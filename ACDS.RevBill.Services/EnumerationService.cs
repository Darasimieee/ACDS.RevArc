using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Property;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NPOI.Util;

namespace ACDS.RevBill.Services
{
    internal sealed class EnumerationService : IEnumerationService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        private readonly PID _pidConfig;
        private DataContext _context;
        public JsonModelService _modelService;
        public IEnumerable<VerifyPidResponseDto>? VerifyPidResponse;
        public IEnumerable<GetTaxPayerByPhoneNumberResponseDto>? GetTaxPayerByPhoneNumber;
        public IEnumerable<GetTaxPayerByNameResponseDto>? GetTaxPayerByName;
        public IEnumerable<GetTaxPayerByEmailResponseDto>? GetTaxPayerByEmail;
        public IEnumerable<CorporatePayerIDResponse>? GenerateCorporatePID;
        private string? Email { get; set; }

        public EnumerationService(IRepositoryManager repository, ILoggerManager logger,ILoggerManager loggerManager, IMapper mapper, DataContext context,
            PID pidConfig, JsonModelService modelService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _pidConfig = pidConfig;
            _modelService = modelService;
            _loggerManager = loggerManager;
        }

        public async Task<(IEnumerable<GetBusinessProfileDto> businessProfile, MetaData metaData)> GetAllBusinessProfilesAsync(int organisationId,
          DefaultParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var businessProfilesWithMetaData = await _repository.BusinessProfile.GetAllBusinessProfilesAsync(organisationId, requestParameters, trackChanges);

            var businessProfileDto = _mapper.Map<IEnumerable<GetBusinessProfileDto>>(businessProfilesWithMetaData);

            return (businessProfile: businessProfileDto, metaData: businessProfilesWithMetaData.MetaData);
        }

        public async Task<GetBusinessProfileDto> GetBusinessProfileAsync(int organisationId, int businessProfileId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var businessProfile = await _repository.BusinessProfile.GetBusinessProfileAsync(organisationId, businessProfileId, trackChanges);
            //check if the business profile is null
            if (businessProfile is null)
                throw new IdNotFoundException("business profile", businessProfileId);
           
            var businessProfileDto = _mapper.Map<GetBusinessProfileDto>(businessProfile);

            return businessProfileDto;
        }

        public async Task<(IEnumerable<GetPropertiesDto> properties, MetaData metaData)> GetAllPropertiesAsync(int organisationId,
            PropertyParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            if (!requestParameters.ValidDateRange)
                throw new MaxDateRangeBadRequestException();

            var propertiesWithMetaData = await _repository.Property.GetAllPropertiesAsync(organisationId, requestParameters, trackChanges);

            var propertiesDto = _mapper.Map<IEnumerable<GetPropertiesDto>>(propertiesWithMetaData);

            return (properties: propertiesDto, metaData: propertiesWithMetaData.MetaData);


        }
        public async Task<(IEnumerable<GetPropertiesDto> properties, MetaData metaData)> GetPropertiesbyAgencyAsync(int organisationId, int agencyId,
         PropertyParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            if (!requestParameters.ValidDateRange)
                throw new MaxDateRangeBadRequestException();

            var propertiesWithMetaData = await _repository.Property.GetPropertiesbyAgencyAsync(organisationId, agencyId, requestParameters, trackChanges);

            var propertiesDto = _mapper.Map<IEnumerable<GetPropertiesDto>>(propertiesWithMetaData);

            return (properties: propertiesDto, metaData: propertiesWithMetaData.MetaData);
        }

        public async Task<GetPropertiesDto> GetPropertyAsync(int organisationId, int propertyId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var property = await _repository.Property.GetPropertyAsync(organisationId, propertyId, trackChanges);

            var propertyDto = _mapper.Map<GetPropertiesDto>(property);

            return propertyDto;
        }

        public async Task<Response> CreatePropertyAsync(int organisationId, CreatePropertyDto createProperty)
        {
            Response resp = new Response();

            var propertyEntity = _mapper.Map<Property>(createProperty);

            var propertyExits = await CheckPropertExist(organisationId, createProperty);

            if (propertyExits == 0)
            {
                try {
                    _repository.Property.CreateProperty(organisationId, propertyEntity);

                    await _repository.SaveAsync();
                    //var propertyToReturn = _mapper.Map<GetPropertiesDto>(propertyEntity);
                    resp.Status = 200;
                    resp.StatusMessage = "Property succesfully created";
                    resp.Data = propertyEntity.PropertyId;
                } catch(Exception ex) {
                
                }



            }
            else
            {
                resp.Status = 409;
                resp.StatusMessage = "Property Exists !";

            }

            return resp;
        }
        public async Task<int> CheckPropertExist(int organisationId, CreatePropertyDto createProperty)
        {
            Response resp = new Response();
            var properties = new List<Property>();
            try {
                properties = _context.Properties.Where(x => x.OrganisationId.Equals(organisationId) && x.AgencyId.Equals(createProperty.AgencyId) && x.StreetId.Equals(createProperty.StreetId)).ToList();
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
            

            var result = properties.Where(x => x.BuildingNo.Equals(createProperty.BuildingNo) && x.BuildingName.Equals(createProperty.BuildingName) && x.LocationAddress.Equals(createProperty.LocationAddress)).ToList();
            var precise = result.Where(x => x.SpaceIdentifierId.Equals(createProperty.SpaceIdentifierId) && x.SpaceFloor.Equals(createProperty.SpaceFloor)).ToList();
            if (precise.Any())
            {
                return precise.FirstOrDefault().PropertyId;

            }
            return 0;
        }

        public async Task UpdatePropertyAsync(int organisationId, int propertyId, PropertyUpdateDto propertyUpdate, bool trackChanges)
        {
            var propertyEntity = await _repository.Property.GetPropertyAsync(organisationId, propertyId, trackChanges);
            if (propertyEntity is null)
                throw new IdNotFoundException("property", propertyId);

            _mapper.Map(propertyUpdate, propertyEntity);
            await _repository.SaveAsync();
        }

        //public async Task<IEnumerable<GetWardDto>> GetAllWardsAsync(int organisationId, bool trackChanges)
        //{
        //    await CheckIfOrganisationExists(organisationId, trackChanges);

        //    var wards = await _repository.Wards.GetAllWardsAsync(organisationId, trackChanges);

        //    var wardDto = _mapper.Map<IEnumerable<GetWardDto>>(wards);

        //    return wardDto;
        //}

        //public async Task<GetWardDto> GetWardAsync(int organisationId, int wardId, bool trackChanges)
        //{
        //    await CheckIfOrganisationExists(organisationId, trackChanges);

        //    var wards = await _repository.Wards.GetWardAsync(organisationId, wardId, trackChanges);
        //    if (wards is null)
        //        throw new IdNotFoundException("ward", wardId);

        //    var wardDto = _mapper.Map<GetWardDto>(wards);

        //    return wardDto;
        //}

        //public async Task<GetWardDto> CreateWardAsync(int organisationId, CreateWardDto createWard)
        //{
        //    var wardEntity = _mapper.Map<Ward>(createWard);

        //    _repository.Wards.CreateWard(organisationId, wardEntity);
        //    await _repository.SaveAsync();

        //    var wardToReturn = _mapper.Map<GetWardDto>(wardEntity);

        //    return wardToReturn;
        //}

        //public async Task UpdateWardAsync(int organisationId, int Id, UpdateWardDto updateWard, bool trackChanges)
        //{
        //    var wardEntity = await _repository.Wards.GetWardAsync(organisationId, Id, trackChanges);
        //    if (wardEntity is null)
        //        throw new IdNotFoundException("ward", Id);

        //    _mapper.Map(updateWard, wardEntity);
        //    await _repository.SaveAsync();
        //}

        public async Task<IEnumerable<GetSpaceIdentifierDto>> GetAllSpaceIdentifiersAsync(int organisationId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var spaceIdentifier = await _repository.SpaceIdentifier.GetAllSpaceIdentifiersAsync(organisationId, trackChanges);

            var spaceIdentifierDto = _mapper.Map<IEnumerable<GetSpaceIdentifierDto>>(spaceIdentifier);

            return spaceIdentifierDto;
        }

        public async Task<GetSpaceIdentifierDto> GetSpaceIdentifierAsync(int organisationId, int spaceIdentifierId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var spaceIdentifier = await _repository.SpaceIdentifier.GetSpaceIdentifierAsync(organisationId, spaceIdentifierId, trackChanges);
            if (spaceIdentifier is null)
                throw new IdNotFoundException("space identifier", spaceIdentifierId);

            var spaceIdentifierDto = _mapper.Map<GetSpaceIdentifierDto>(spaceIdentifier);

            return spaceIdentifierDto;
        }

        public async Task<GetSpaceIdentifierDto> CreateSpaceIdentifierAsync(int organisationId, CreateSpaceIdentifierDto createSpaceIdentifier)
        {
            var spaceIdentifierdEntity = _mapper.Map<SpaceIdentifier>(createSpaceIdentifier);

            _repository.SpaceIdentifier.CreateSpaceIdentifier(organisationId, spaceIdentifierdEntity);
            await _repository.SaveAsync();

            var spaceIdentifierdToReturn = _mapper.Map<GetSpaceIdentifierDto>(spaceIdentifierdEntity);

            return spaceIdentifierdToReturn;
        }

        public async Task UpdateSpaceIdentifierAsync(int organisationId, int spaceIdentifierId, UpdateSpaceIdentifierDto updateSpaceIdentifier, bool trackChanges)
        {
            var spaceIdentifierdEntity = await _repository.SpaceIdentifier.GetSpaceIdentifierAsync(organisationId, spaceIdentifierId, trackChanges);
            if (spaceIdentifierdEntity is null)
                throw new IdNotFoundException("space identifier", spaceIdentifierId);

            _mapper.Map(updateSpaceIdentifier, spaceIdentifierdEntity);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<GetBusinessTypeDto>> GetAllBusinessTypesAsync(int organisationId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var businessType = await _repository.BusinessType.GetAllBusinessTypesAsync(organisationId, trackChanges);

            var businessTypeDto = _mapper.Map<IEnumerable<GetBusinessTypeDto>>(businessType);

            return businessTypeDto;
        }

        public async Task<GetBusinessTypeDto> GetBusinessTypeAsync(int organisationId, int businessTypeId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var businessType = await _repository.BusinessType.GetBusinessTypeAsync(organisationId, businessTypeId, trackChanges);
            if (businessType is null)
                throw new IdNotFoundException("business type", businessTypeId);

            var businessTypeDto = _mapper.Map<GetBusinessTypeDto>(businessType);

            return businessTypeDto;
        }

        public async Task<GetBusinessTypeDto> CreateBusinessTypeAsync(int organisationId, CreateBusinessTypeDto createBusinessType)
        {
            var businessTypeEntity = _mapper.Map<BusinessType>(createBusinessType);

            _repository.BusinessType.CreateBusinessType(organisationId, businessTypeEntity);
            await _repository.SaveAsync();

            var businessTypeToReturn = _mapper.Map<GetBusinessTypeDto>(businessTypeEntity);

            return businessTypeToReturn;
        }

        public async Task UpdateBusinessTypeAsync(int organisationId, int businessTypeId, UpdateBusinessTypeDto updateBusinessType, bool trackChanges)
        {
            var businessTypeEntity = await _repository.BusinessType.GetBusinessTypeAsync(organisationId, businessTypeId, trackChanges);
            if (businessTypeEntity is null)
                throw new IdNotFoundException("business type", businessTypeId);

            _mapper.Map(updateBusinessType, businessTypeEntity);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<GetBusinessSizeDto>> GetAllBusinessSizesAsync(int organisationId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var businessSize = await _repository.BusinessSize.GetAllBusinessSizesAsync(organisationId, trackChanges);

            var businessSizeDto = _mapper.Map<IEnumerable<GetBusinessSizeDto>>(businessSize);

            return businessSizeDto;
        }

        public async Task<GetBusinessSizeDto> GetBusinessSizeAsync(int organisationId, int businessSizeId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var businessSize = await _repository.BusinessSize.GetBusinessSizeAsync(organisationId, businessSizeId, trackChanges);
            if (businessSize is null)
                throw new IdNotFoundException("business size", businessSizeId);

            var businessSizeDto = _mapper.Map<GetBusinessSizeDto>(businessSize);

            return businessSizeDto;
        }

        public async Task<GetBusinessSizeDto> CreateBusinessSizeAsync(int organisationId, CreateBusinessSizeDto createBusinessSize)
        {
            var businessSizeEntity = _mapper.Map<BusinessSize>(createBusinessSize);

            _repository.BusinessSize.CreateBusinessSize(organisationId, businessSizeEntity);
            await _repository.SaveAsync();

            var businessSizeToReturn = _mapper.Map<GetBusinessSizeDto>(businessSizeEntity);

            return businessSizeToReturn;
        }

        public async Task UpdateBusinessSizeAsync(int organisationId, int businessSizeId, UpdateBusinessSizeDto updateBusinessSize, bool trackChanges)
        {
            var businessSizeEntity = await _repository.BusinessSize.GetBusinessSizeAsync(organisationId, businessSizeId, trackChanges);
            if (businessSizeEntity is null)
                throw new IdNotFoundException("business size", businessSizeId);

            _mapper.Map(updateBusinessSize, businessSizeEntity);
            await _repository.SaveAsync();
        }
        public Response VerifyAgencyCode(string agencycode)
        {
            Response response = new();
            var agencyhead = agencycode.Substring(0, 3);
            var agencies =  _modelService.GetEBSAgencyByCode(agencyhead);
            if (agencies.Result.Count() == 0)
            {
                response.StatusMessage = "Agency does not exist!";
                response.Status = 404;
            }
            else
            {
                response.StatusMessage = "Agency exists!";
                response.Status = 200;
            }
            return response;
        }

        public Response VerifyPid(PayerIdEnumerationDto customer)
        {
            var pidEntity = _mapper.Map<PayerIdEnumerationDto>(customer);
            VerifyPidResponse = _modelService.VerifyPayerId(pidEntity);


            VerifyPidResponseDto verifyPidResponse = new();

            foreach (var response in VerifyPidResponse)
            {
                verifyPidResponse.Address = response.Address;
                verifyPidResponse.FirstName = response.FirstName;
                verifyPidResponse.SurName = response.SurName;
                verifyPidResponse.MiddleName = response.MiddleName;
                verifyPidResponse.GSM = response.GSM;
                verifyPidResponse.Email = response.Email;
                verifyPidResponse.PayerID = response.PayerID;
                verifyPidResponse.CorporateName = response.CorporateName;
                verifyPidResponse.birthdate = response.birthdate;
                verifyPidResponse.BranchName = response.BranchName;
                verifyPidResponse.Title = response.Title;
                verifyPidResponse.NIN_Exist = response.NIN_Exist;
                verifyPidResponse.BVN_Exist = response.BVN_Exist;
                verifyPidResponse.FullName = response.FullName;
                verifyPidResponse.Sex = response.Sex;
                verifyPidResponse.maritalstatus = response.maritalstatus;
                verifyPidResponse.CorpID = response.CorpID;
            }

            var verifiedPid = _mapper.Map<VerifyPidResponseDto>(verifyPidResponse);

            Response dataResponse = new();

            if (VerifyPidResponse.Count() > 0)
            {
                dataResponse.StatusMessage = "PayerID Record Found";
                dataResponse.Data = verifiedPid;
                dataResponse.Status = 200;
            }

            else
            {
                dataResponse.StatusMessage = "PayerID Record Not Found";
                dataResponse.Status = 404;
            }

            return dataResponse;
        }

        public async Task<Response> EnumerationAsync(int organisationId, CompleteEnumerationParams enumeration, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);
            var propertyExists = await CheckPropertExist(organisationId, enumeration.CreatePropertyDto);
            var checkIfCustomerExists = await _context.Customers.Where(c => (c.Email == enumeration.CreateCustomerDto.Email || c.PhoneNo == enumeration.CreateCustomerDto.PhoneNo) && c.OrganisationId == organisationId).ToListAsync();

            List<CustomerProperty> customerproperty = new List<CustomerProperty>();
            //create object to store property and customer ID.
            Property_Customer property_Customer = new();
            Response dataResponse = new Response();

            //map property
            var propertyEntity = _mapper.Map<Property>(enumeration.CreatePropertyDto);
            if (propertyExists != 0 && checkIfCustomerExists.Count() != 0)
            {
                customerproperty = await _context.CustomerProperties.Where(e => e.PropertyId == propertyExists && e.CustomerId == checkIfCustomerExists.FirstOrDefault().CustomerId).ToListAsync();
                if (customerproperty.Count() != 0)
                {
                    dataResponse.Status = 409;
                    dataResponse.StatusMessage = "Property Exists, customer exists in the property!";
                    return dataResponse;
                }
                propertyEntity.PropertyId = propertyExists;
            }
            List<CreateBusinessProfileDto> createBusinessprofile = new List<CreateBusinessProfileDto>();

            foreach (var i in enumeration.CreateBusinessProfileDto)
            {
                CreateBusinessProfileDto createBusinessprof = new CreateBusinessProfileDto();
                foreach (var item in i.BillRevenues)
                {
                    createBusinessprof.RevenueId = item;
                    createBusinessprof.BusinessSizeId = i.BusinessSizeId;
                    createBusinessprof.BusinessTypeId = i.BusinessTypeId;
                    createBusinessprof.DateCreated = i.DateCreated;
                    createBusinessprof.CreatedBy = i.CreatedBy;
                    createBusinessprofile.Add(createBusinessprof);
                }


            }

            //map business profile
            var businessProfileEntity = _mapper.Map<IEnumerable<BusinessProfile>>(createBusinessprofile);

            //map customer
            var customerEntity = _mapper.Map<Customers>(enumeration.CreateCustomerDto);

            //map customer property
            var customerPropertyEntity = _mapper.Map<CustomerProperty>(enumeration.CreateCustomerPropertyDto);

            //concatenate to get full name
            customerEntity.FullName = $"{customerEntity.FirstName} {customerEntity.MiddleName} {customerEntity.LastName}";


            if (checkIfCustomerExists.Count != 0)
            {
                enumeration.CreateCustomerPropertyDto.DoesCustomerExist = true;
                customerEntity.CustomerId = checkIfCustomerExists.FirstOrDefault().CustomerId;
            }


            //if the customer does not supply details, generate information for them
            if (enumeration.CreateCustomerDto.SuppliedPID == false)
            {
                //generate random phone number
                var randomPhoneNumber = EnumerationUtility.GenerateNigerianPhoneNumber();
                bool phoneNumberExists = _context.Customers.Any(c => c.PhoneNo == randomPhoneNumber);

                while (phoneNumberExists)
                {
                    // phone number already exists, generate a new random number
                    randomPhoneNumber = EnumerationUtility.GenerateNigerianPhoneNumber();
                    phoneNumberExists = _context.Customers.Any(c => c.PhoneNo == randomPhoneNumber);
                }

                var fullName = $"{enumeration.CreatePropertyDto.BuildingNo} {enumeration.CreatePropertyDto.BuildingName}";

                //remove space from building name for email
                if (propertyEntity.BuildingName.Contains(" "))
                {
                    Email = propertyEntity.BuildingName.Replace(" ", string.Empty);
                }

                //phoneNumberExists is false, execute the code in the if block
                CustomerEnumerationDto generatePID = new CustomerEnumerationDto();
                generatePID.FirstName = fullName;
                generatePID.Email = $"{propertyEntity.BuildingNo}.{Email}@gmail.com";
                generatePID.Phone = randomPhoneNumber;
                generatePID.LastName = propertyEntity.BuildingName;
                generatePID.Title = "Mr";
                generatePID.MaritalStatus = "S";
                generatePID.Sex = "M";
                generatePID.Address = propertyEntity.LocationAddress;
                generatePID.DateOfBirth = "08/05/2000";
                generatePID.Type = "N";

                var pidEntity = _mapper.Map<CustomerEnumerationDto>(generatePID);
                var pidCreationUrl = $"{_pidConfig.BASE_URL}/Interface/pidcreation";
                var pidCreationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + generatePID.Phone + generatePID.Email + generatePID.Address + _pidConfig.STATE);

                //map hash, state and client id
                pidEntity.Hash = pidCreationHash;
                pidEntity.State = _pidConfig.STATE;
                pidEntity.ClientId = _pidConfig.CLIENT_ID;

                //generate PID
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, pidCreationUrl);

                //serialize payload 
                var payload = JsonConvert.SerializeObject(pidEntity);
                var content = new StringContent(payload, null, "application/json");
                request.Content = content;

                //get response
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

                //deserialise response
                PIDResponse output = JsonConvert.DeserializeObject<PIDResponse>(responseContent);
                if (output.Status == "FAILED")
                {
                    dataResponse.StatusMessage = output.Status;
                    dataResponse.Data = output;
                    dataResponse.Status = 401;

                    return dataResponse;
                }

                else
                {
                    dataResponse.StatusMessage = output.Status;
                    dataResponse.Status = 200;
                    dataResponse.Data = output;

                    //extract payer id
                    string extractedPayerID = EnumerationUtility.ExtractPayerID(output.Pid);

                    //split property name to get first, last name and email
                    customerEntity.FirstName = $"{propertyEntity.BuildingNo} {propertyEntity.BuildingName}";
                    customerEntity.LastName = $"{propertyEntity.BuildingNo} {propertyEntity.BuildingName}";
                    customerEntity.Email = $"{propertyEntity.BuildingName}{propertyEntity.BuildingNo}";
                    customerEntity.PhoneNo = randomPhoneNumber;
                    customerEntity.GenderId = 1;
                    customerEntity.MaritalStatusId = 1;
                    customerEntity.TitleId = 1;
                    customerEntity.PayerTypeId = 1;
                    customerEntity.PayerId = extractedPayerID;
                    customerEntity.Address = propertyEntity.LocationAddress;

                    //save to property table
                    _repository.Property.CreateProperty(organisationId, propertyEntity);

                    //save to retrieve generated property id
                    await _repository.SaveAsync();

                    //save to retrieve customer id
                    _repository.Customer.CreateCustomer(organisationId, customerEntity);

                    await _repository.SaveAsync();

                    _repository.BusinessProfile.CreateMultipleBusinessProfiles(organisationId, propertyEntity.PropertyId, customerEntity.CustomerId, businessProfileEntity);
                    _repository.CustomerProperty.CreateCustomerProperty(organisationId, propertyEntity.PropertyId, customerEntity.CustomerId, customerPropertyEntity);

                    await _repository.SaveAsync();


                    property_Customer.CustomerID = customerEntity.CustomerId;
                    property_Customer.PropertyID = propertyEntity.PropertyId;
                    property_Customer.CustomerName = customerEntity.FullName;
                    property_Customer.PropertyName = $"{propertyEntity.BuildingNo}, {propertyEntity.BuildingName}";

                    dataResponse.StatusMessage = "Enumeration Successful";
                    dataResponse.Status = 200;
                    dataResponse.Data = property_Customer;
                }
            }

            //if new customer
            if (propertyExists == 0 && enumeration.CreateCustomerPropertyDto.DoesCustomerExist == false && enumeration.CreateCustomerDto.SuppliedPID != false)
            {

                //save to property table
                _repository.Property.CreateProperty(organisationId, propertyEntity);

                //save to retrieve generated property id
                await _repository.SaveAsync();


                //save to retrieve customer id
                _repository.Customer.CreateCustomer(organisationId, customerEntity);

                await _repository.SaveAsync();

                _repository.BusinessProfile.CreateMultipleBusinessProfiles(organisationId, propertyEntity.PropertyId, customerEntity.CustomerId, businessProfileEntity);

                _repository.CustomerProperty.CreateCustomerProperty(organisationId, propertyEntity.PropertyId, customerEntity.CustomerId, customerPropertyEntity);

                await _repository.SaveAsync();


                property_Customer.CustomerID = customerEntity.CustomerId;
                property_Customer.PropertyID = propertyEntity.PropertyId;
                property_Customer.CustomerName = customerEntity.FullName;
                property_Customer.PropertyName = $"{propertyEntity.BuildingNo}, {propertyEntity.BuildingName}";

                dataResponse.StatusMessage = "Enumeration Successful";
                dataResponse.Status = 200;
                dataResponse.Data = property_Customer;
            }

            //if customer exists already
            else if (enumeration.CreateCustomerPropertyDto.DoesCustomerExist == true && enumeration.CreateCustomerDto.SuppliedPID != false)
            {
                if (propertyExists == 0)
                {
                    //save to property table
                    _repository.Property.CreateProperty(organisationId, propertyEntity);

                    //save to retrieve generated property id
                    await _repository.SaveAsync();
                }
                _repository.BusinessProfile.CreateMultipleBusinessProfiles(organisationId, propertyEntity.PropertyId, customerEntity.CustomerId, businessProfileEntity);
                _repository.CustomerProperty.CreateCustomerProperty(organisationId, propertyEntity.PropertyId, customerEntity.CustomerId, customerPropertyEntity);

                await _repository.SaveAsync();


                property_Customer.PropertyID = propertyEntity.PropertyId;
                property_Customer.PropertyName = $"{propertyEntity.BuildingNo}, {propertyEntity.BuildingName}";

                if (enumeration.CreateCustomerPropertyDto.DoesCustomerExist == true)
                {
                    property_Customer.CustomerID = customerPropertyEntity.CustomerId;
                    //get customer details

                    property_Customer.CustomerName = checkIfCustomerExists.FirstOrDefault().FirstName + " " + checkIfCustomerExists.FirstOrDefault().MiddleName + " " + checkIfCustomerExists.FirstOrDefault().LastName;
                }

                else
                {
                    property_Customer.CustomerID = customerEntity.CustomerId;
                    property_Customer.CustomerName = customerEntity.FullName;
                }



                dataResponse.StatusMessage = "Enumeration Successful";
                dataResponse.Status = 200;
                dataResponse.Data = property_Customer;
            }


            return dataResponse;
        }

        public async Task<Response> EnumerationWhenPropertyExistsAsync(int organisationId, int propertyId, PartialEnumerationParams enumeration, bool trackChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackChanges);
            await CheckIfPropertyExists(organisationId, propertyId, trackChanges);

            //map business profile
            var businessProfileEntity = _mapper.Map<IEnumerable<BusinessProfile>>(enumeration.CreateBusinessProfileDto);

            //map customer
            var customerEntity = _mapper.Map<Customers>(enumeration.CreateCustomerDto);

            //map customer property
            var customerPropertyEntity = _mapper.Map<CustomerProperty>(enumeration.CreateCustomerPropertyDto);

            //concatenate to get full name
            customerEntity.FullName = $"{customerEntity.FirstName} {customerEntity.MiddleName} {customerEntity.LastName}";

            var checkIfEmailExists = _context.Customers.Any(c => c.Email == customerEntity.Email);
            var checkIfPhoneNumberExists = _context.Customers.Any(c => c.PhoneNo == customerEntity.PhoneNo);

            if (checkIfEmailExists || checkIfPhoneNumberExists)
            {
                dataResponse.StatusMessage = $"Customer with email: {customerEntity.Email} and phone number: {customerEntity.PhoneNo} already exists)";
                dataResponse.Status = 400;
            }

            else
            {
                //if new customer
                if (enumeration.CreateCustomerPropertyDto.DoesCustomerExist == false)
                {
                    _repository.Customer.CreateCustomer(organisationId, customerEntity);

                    await _repository.SaveAsync();

                    _repository.BusinessProfile.CreateMultipleBusinessProfiles(organisationId, propertyId, customerEntity.CustomerId, businessProfileEntity);
                    _repository.CustomerProperty.CreateCustomerProperty(organisationId, propertyId, customerEntity.CustomerId, customerPropertyEntity);

                    await _repository.SaveAsync();
                }

                //if customer exists already
                else if (enumeration.CreateCustomerPropertyDto.DoesCustomerExist == true)
                {
                    _repository.BusinessProfile.CreateMultipleBusinessProfiles(organisationId, propertyId, customerPropertyEntity.CustomerId, businessProfileEntity);
                    _repository.CustomerProperty.CreateCustomerProperty(organisationId, propertyId, customerPropertyEntity.CustomerId, customerPropertyEntity);

                    await _repository.SaveAsync();
                }

                //get property details
                var property = await _context.Properties.Where(x => x.OrganisationId == organisationId && x.PropertyId == propertyId)
                                  .Select(x => x.BuildingNo + ", " + x.BuildingName)
                                  .FirstOrDefaultAsync();

                //create object to store property and customer ID.
                Property_Customer property_Customer = new();
                property_Customer.PropertyID = propertyId;
                property_Customer.PropertyName = property;

                if (enumeration.CreateCustomerPropertyDto.DoesCustomerExist == true)
                {
                    property_Customer.CustomerID = customerPropertyEntity.CustomerId;
                    //get customer details
                    var customer = await _context.Customers.Where(x => x.CustomerId == customerPropertyEntity.CustomerId).Select(x => x).FirstOrDefaultAsync();
                    property_Customer.CustomerName = customer.FirstName + " " + customer.MiddleName + " " + customer.LastName;
                }

                else
                {
                    property_Customer.CustomerID = customerEntity.CustomerId;
                    property_Customer.CustomerName = customerEntity.FullName;
                }

                dataResponse.StatusMessage = "Enumeration Successful";
                dataResponse.Status = 200;
                dataResponse.Data = property_Customer;
            }

            return dataResponse;
        }

        public Response PayerIdSearchByPhoneNumber(GetTaxPayerRequestDto getTaxPayer)
        {
            var pidEntity = _mapper.Map<GetTaxPayerRequestDto>(getTaxPayer);
            GetTaxPayerByPhoneNumber = _modelService.GetCustomerDetailsByPhoneNumber(pidEntity);

            GetTaxPayerByPhoneNumberResponseDto getTaxPayerByPhone = new();

            foreach (var response in GetTaxPayerByPhoneNumber)
            {
                getTaxPayerByPhone.Address = response.Address;
                getTaxPayerByPhone.PayerID = response.PayerID;
                getTaxPayerByPhone.Title = response.Title;
                getTaxPayerByPhone.FullName = response.FullName;
                getTaxPayerByPhone.FirstName = response.FirstName;
                getTaxPayerByPhone.MiddleName = response.MiddleName;
                getTaxPayerByPhone.SurName = response.SurName;
                getTaxPayerByPhone.BirthDate = response.BirthDate;
                getTaxPayerByPhone.Email = response.Email;
                getTaxPayerByPhone.GSM = response.GSM;
                getTaxPayerByPhone.MaritalStatus = response.MaritalStatus;
                getTaxPayerByPhone.Nationality = response.Nationality;
                getTaxPayerByPhone.Sex = response.Sex;
            }

            var searchEntity = _mapper.Map<GetTaxPayerByPhoneNumberResponseDto>(getTaxPayerByPhone);

            Response dataResponse = new();

            if (getTaxPayerByPhone.PayerID != null)
            {
                dataResponse.StatusMessage = "PayerID Record Found";
                dataResponse.Data = searchEntity;
                dataResponse.Status = 200;
            }

            else
            {
                dataResponse.StatusMessage = "PayerID Record Not Found";
                dataResponse.Status = 404;
                dataResponse.Data = null;
            }

            return dataResponse;
        }

        public Response PayerIdSearchByName(GetTaxPayerRequestDto getTaxPayer)
        {
            var pidEntity = _mapper.Map<GetTaxPayerRequestDto>(getTaxPayer);
            GetTaxPayerByName = _modelService.GetCustomerDetailsByName(pidEntity);

            GetTaxPayerByNameResponseDto getTaxPayerByName = new();
            List<object> pidResult = new List<object>();

            foreach (var response in GetTaxPayerByName)
            {
                //add to list
                pidResult.Add(response);

                getTaxPayerByName.Address = response.Address;
                getTaxPayerByName.Title = response.Title;
                getTaxPayerByName.FullName = response.FullName;
                getTaxPayerByName.SurName = response.SurName;
                getTaxPayerByName.Email = response.Email;
                getTaxPayerByName.GSM = response.GSM;
                getTaxPayerByName.PersID_abc = response.PersID_abc;
                getTaxPayerByName.Exact2 = response.Exact2;
                getTaxPayerByName.CorporateID = response.CorporateID;
                getTaxPayerByName.BVN = response.BVN;
                getTaxPayerByName.NIN = response.NIN;
            }

            Response dataResponse = new();

            if (getTaxPayerByName.FullName != null)
            {
                dataResponse.StatusMessage = "PayerID Record Found";
                dataResponse.Data = pidResult;
                dataResponse.Status = 200;
            }

            else
            {
                dataResponse.StatusMessage = "PayerID Record Not Found";
                dataResponse.Status = 404;
                dataResponse.Data = null;
            }

            return dataResponse;
        }

        public Response PayerIdSearchByEmail(GetTaxPayerRequestDto getTaxPayer)
        {
            var pidEntity = _mapper.Map<GetTaxPayerRequestDto>(getTaxPayer);
            GetTaxPayerByEmail = _modelService.GetCustomerDetailsByEmail(pidEntity);

            GetTaxPayerByEmailResponseDto getTaxPayerByEmail = new();

            foreach (var response in GetTaxPayerByEmail)
            {
                getTaxPayerByEmail.Address = response.Address;
                getTaxPayerByEmail.PayerID = response.PayerID;
                getTaxPayerByEmail.Title = response.Title;
                getTaxPayerByEmail.FullName = response.FullName;
                getTaxPayerByEmail.FirstName = response.FirstName;
                getTaxPayerByEmail.MiddleName = response.MiddleName;
                getTaxPayerByEmail.SurName = response.SurName;
                getTaxPayerByEmail.BirthDate = response.BirthDate;
                getTaxPayerByEmail.Email = response.Email;
                getTaxPayerByEmail.GSM = response.GSM;
                getTaxPayerByEmail.MaritalStatus = response.MaritalStatus;
                getTaxPayerByEmail.Nationality = response.Nationality;
                getTaxPayerByEmail.Sex = response.Sex;
            }

            var searchEntity = _mapper.Map<GetTaxPayerByEmailResponseDto>(getTaxPayerByEmail);

            Response dataResponse = new();

            if (getTaxPayerByEmail.PayerID != null)
            {
                dataResponse.StatusMessage = "PayerID Record Found";
                dataResponse.Data = searchEntity;
                dataResponse.Status = 200;
            }

            else
            {
                dataResponse.StatusMessage = "PayerID Record Not Found";
                dataResponse.Status = 404;
                dataResponse.Data = null;
            }

            return dataResponse;
        }

        public async Task<Response> CreatePIDWithBioData(CustomerEnumerationDto customer)
        {
            _loggerManager.LogInfo($"converting to hash and picking url  from Mr Qazim code for customer {customer.Email}");
            _logger.LogInfo("I got to interface");
            var pidEntity = _mapper.Map<CustomerEnumerationDto>(customer);
            var pidCreationUrl = $"{_pidConfig.BASE_URL}/FCTRS/Interface/pidcreation";
            var pidCreationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + pidEntity.Phone + pidEntity.Email + pidEntity.Address + _pidConfig.STATE);


            _loggerManager.LogInfo($"mapping the hash, state id and clientid for customer {customer.Email}");
            _logger.LogInfo("I got to hash");
            //map hash, state and client id
            pidEntity.Hash = pidCreationHash;
            pidEntity.State = _pidConfig.STATE;
            pidEntity.ClientId = _pidConfig.CLIENT_ID;


            _loggerManager.LogInfo($"generate PID for customer {customer.Email}");
            _logger.LogInfo("I got create PID");
            //generate PID
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, pidCreationUrl);

            _loggerManager.LogInfo($"serialize payload for customer {customer.Email}");
            _logger.LogInfo("I got to payload");
            //serialize payload 
            var payload = JsonConvert.SerializeObject(pidEntity);
            var content = new StringContent(payload, null, "application/json");
            request.Content = content;

            //get response
            _loggerManager.LogInfo($"response for customer {customer.Email}");
            _logger.LogInfo("I got to response for customer");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

            //deserialise response
            _loggerManager.LogInfo($"return output for customer {customer.Email}");
            _logger.LogInfo("I got to PID response");
            PIDResponse output = JsonConvert.DeserializeObject<PIDResponse>(responseContent);
            _loggerManager.LogInfo($"return output for customer {customer.Email}");
            _loggerManager.LogInfo(output.Pid + output.StatusMessage);
            _logger.LogInfo(output.Pid + output.StatusMessage);


            Response dataResponse = new();

            if (output.Status == "FAILED")
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Data = output;
                dataResponse.Status = 401;
            }

            else
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Status = 200;
                dataResponse.Data = output;
            }

            return dataResponse;
        }

        public async Task<Response> CreatePIDWithBVN(CustomerEnumerationBVNDto customer)
        {
            var pidEntity = _mapper.Map<CustomerEnumerationBVNDto>(customer);

            var pidCreationUrl = $"{_pidConfig.BASE_URL}/Interface/CreatePidBvn";
            var pidCreationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + customer.Type + customer.PhoneNumber + customer.Address + _pidConfig.STATE);

            //map hash, state and client id
            pidEntity.Hash = pidCreationHash;
            pidEntity.State = _pidConfig.STATE;
            pidEntity.ClientId = _pidConfig.CLIENT_ID;

            //generate PID
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, pidCreationUrl);

            //serialize payload 
            var payload = JsonConvert.SerializeObject(pidEntity);
            var content = new StringContent(payload, null, "application/json");
            request.Content = content;

            //get response
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

            //deserialise response
            PIDResponse output = JsonConvert.DeserializeObject<PIDResponse>(responseContent);
            Response dataResponse = new();

            if (output.Status == "FAILED")
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Data = output;
                dataResponse.Status = 401;
            }

            else
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Status = 200;
                dataResponse.Data = output;
            }

            return dataResponse;
        }

        public async Task<Response> CreatePIDWithNIN(CustomerEnumerationNINDto customer)
        {
            try
            { 

            }catch (Exception ex) 
            { 

            }
            var pidEntity = _mapper.Map<CustomerEnumerationNINDto>(customer);

            var pidCreationUrl = $"{_pidConfig.BASE_URL}/FCTRS/Interface/CreatePidNinPhoneO";
            var pidCreationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + customer.Type + customer.PhoneNumber + customer.Address + _pidConfig.STATE);

            //map hash, state and client id
            pidEntity.Hash = pidCreationHash;
            pidEntity.State = _pidConfig.STATE;
            pidEntity.ClientId = _pidConfig.CLIENT_ID;

            //generate PID
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, pidCreationUrl);

            //serialize payload 
            var payload = JsonConvert.SerializeObject(pidEntity);
            var content = new StringContent(payload, null, "application/json");
            request.Content = content;

            //get response
            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

            //deserialise response
            PIDResponse output = JsonConvert.DeserializeObject<PIDResponse>(responseContent);
            Response dataResponse = new();

            if (output.Status == "FAILED")
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Data = output;
                dataResponse.Status = 401;
            }

            else
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Status = 200;
                dataResponse.Data = output;
            }

            return dataResponse;
        }

        public Response CreateCorporatePID(CorporatePayerIDRequest customer)
        {
            var pidEntity = _mapper.Map<CorporatePayerIDRequest>(customer);
            GenerateCorporatePID = _modelService.CreateCorporatePID(pidEntity);

            CorporatePayerIDResponse payerIDResponse = new();

            foreach (var response in GenerateCorporatePID)
            {
                payerIDResponse.Flag = response.Flag;
                payerIDResponse.outData = response.outData;
            }

            var payerIDResponseEntity = _mapper.Map<CorporatePayerIDResponse>(payerIDResponse);

            Response dataResponse = new();

            //dataResponse.StatusMessage = ;
            dataResponse.Data = payerIDResponseEntity;
            dataResponse.Status = 200;

            return dataResponse;
        }

        public async Task<IEnumerable<GenderDto>> GetAllGendersAsync(bool trackChanges)
        {
            var genders = await _repository.Gender.GetAllGendersAsync(trackChanges);

            var genderDto = _mapper.Map<IEnumerable<GenderDto>>(genders);

            return genderDto;
        }
        public async Task<IEnumerable<Bank_Code>> GetAllBanksAsync()
        {
            var bank_Codes = await _context.Bank_Code.Where(x => x.BankName != null).ToListAsync();
            return bank_Codes;
        }
        public async Task<Response> PushBankcodeAsync()
        {
            var bankcodes = _modelService.GetBankCodes();

            foreach (var bankcode in bankcodes)
            {
                var result = _context.Bank_Code.Where(x => x.BankCode == bankcode.CBNCode).Single();
                if (result == null)
                {
                    var bank_code = new Bank_Code();
                    bank_code.BankName = bankcode.BankName;
                    bank_code.BankCode = bankcode.CBNCode;
                    await _context.Bank_Code.AddAsync(bank_code);
                    await _context.SaveChangesAsync();
                }

            }
            Response response = new Response();
            response.Status = 200;
            response.Data = "";
            response.StatusMessage = "Refresh Successful";
            return response;
        }

        public async Task<IEnumerable<MaritalStatusDto>> GetAllMaritalStatusAsync(bool trackChanges)
        {
            var maritalStatus = await _repository.MaritalStatus.GetAllMaritalStatusAsync(trackChanges);

            var maritalStatusDto = _mapper.Map<IEnumerable<MaritalStatusDto>>(maritalStatus);

            return maritalStatusDto;
        }

        public async Task<IEnumerable<PayerTypeDto>> GetAllPayerTypesAsync(bool trackChanges)
        {
            var payerType = await _repository.PayerType.GetAllPayerTypesAsync(trackChanges);

            var payerTypeDto = _mapper.Map<IEnumerable<PayerTypeDto>>(payerType);

            return payerTypeDto;
        }

        public async Task<IEnumerable<TitleDto>> GetAllTitlesAsync(bool trackChanges)
        {
            var title = await _repository.Title.GetAllTitlesAsync(trackChanges);

            var titleDto = _mapper.Map<IEnumerable<TitleDto>>(title);

            return titleDto;
        }

        public async Task<IEnumerable<LgaDto>> GetAllLgasAsync(bool trackChanges)
        {
            var lga = await _repository.Lgas.GetAllLgasAsync(trackChanges);

            var lgaDto = _mapper.Map<IEnumerable<LgaDto>>(lga);

            return lgaDto;
        }

        public async Task<IEnumerable<LgaDto>> GetLgasByStateAsync(int stateId, bool trackChanges)
        {
            var filteredLga = await _context.LocalGovermentAreas.Where(x => x.StateId.Equals(stateId)).Select(x => x).ToListAsync();

            var lgaDto = _mapper.Map<IEnumerable<LgaDto>>(filteredLga);

            return lgaDto;
        }

        public async Task<IEnumerable<LcdaDto>> GetAllLcdasAsync(bool trackChanges)
        {
            var lcda = await _repository.Lcdas.GetAllLcdasAsync(trackChanges);

            var lcdaDto = _mapper.Map<IEnumerable<LcdaDto>>(lcda);

            return lcdaDto;
        }

        public async Task<IEnumerable<LcdaDto>> GetLcdasByLgaAsync(int lgaId, bool trackChanges)
        {
            var filteredLcda = await _context.LCDAs.Where(x => x.LGAId.Equals(lgaId)).Select(x => x).ToListAsync();

            var lcdaDto = _mapper.Map<IEnumerable<LcdaDto>>(filteredLcda);

            return lcdaDto;
        }

        public async Task<IEnumerable<StateDto>> GetAllStatesAsync(bool trackChanges)
        {
            var state = await _repository.States.GetAllStatesAsync(trackChanges);

            var stateDto = _mapper.Map<IEnumerable<StateDto>>(state);

            return stateDto;
        }

        public async Task<IEnumerable<CountriesDto>> GetAllCountriesAsync(bool trackChanges)
        {
            var countries = await _repository.Countries.GetAllCountriesAsync(trackChanges);

            var countriesDto = _mapper.Map<IEnumerable<CountriesDto>>(countries);

            return countriesDto;
        }

        public async Task<string> NoOfRegisteredPropertiesThisMonth(int organisationId)
        {
            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            int countOfRegisteredProperties = await _context.Properties.Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredProperties.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredPropertiesThisWeek(int organisationId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            int countOfRegisteredProperties = await _context.Properties.Where(x => x.DateCreated >= startOfWeek && x.DateCreated <= endOfWeek && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredProperties.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredPropertiesToday(int organisationId)
        {
            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            int countOfRegisteredProperties = await _context.Properties.Where(x => x.DateCreated >= startOfDay && x.DateCreated <= endOfDay && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredProperties.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredNonPropertiesThisMonth(int organisationId)
        {
            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            int countOfRegisteredNonProperties = await _context.Billing.Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.PropertyId == null && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredNonProperties.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredNonPropertiesThisWeek(int organisationId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            int countOfRegisteredNonProperties = await _context.Billing.Where(x => x.DateCreated >= startOfWeek && x.DateCreated <= endOfWeek && x.PropertyId == null && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredNonProperties.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredNonPropertiesToday(int organisationId)
        {
            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            int countOfRegisteredNonProperties = await _context.Billing.Where(x => x.DateCreated >= startOfDay && x.DateCreated <= endOfDay && x.PropertyId == null && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredNonProperties.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredCustomersThisMonth(int organisationId)
        {
            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            int countOfRegisteredCustomers = await _context.Customers.Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredCustomers.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredCustomersThisWeek(int organisationId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            int countOfRegisteredCustomers = await _context.Customers.Where(x => x.DateCreated >= startOfWeek && x.DateCreated <= endOfWeek && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredCustomers.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfRegisteredCustomersToday(int organisationId)
        {
            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            int countOfRegisteredCustomers = await _context.Customers.Where(x => x.DateCreated >= startOfDay && x.DateCreated <= endOfDay && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfRegisteredCustomers.ToString("N0");

            return formattedNum;
        }

        public async Task<List<AgencySummaryDto>> NoOfPropertiesAndCustomersByAreaOffice(int organisationId)
        {
            var summaryList = await _context.Agencies
                .Where(x => x.OrganisationId == organisationId)
                .Select(a => new
                {
                    Id = 0,
                    AgencyName = a.AgencyName,
                    PropertyCount = _context.Properties.Count(p => p.AgencyId == a.AgencyId),
                    CustomerCount = _context.CustomerProperties.Where(c => c.Property.AgencyId == a.AgencyId).Select(c => c.PropertyId).Count()
                }).ToListAsync();

            List<AgencySummaryDto> result = new List<AgencySummaryDto>();
            foreach (var item in summaryList)
            {
                result.Add(new AgencySummaryDto
                {
                    AgencyName = item.AgencyName,
                    PropertyCount = item.PropertyCount,
                    CustomerCount = item.CustomerCount
                });
            }

            // set the Id field to an auto-incrementing value
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id = i + 1;
            }

            return result;
        }

        public async Task<List<EnumerationManifestDto>> EnumerationManifest(int organisationId, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var summaryList = await _context.BusinessProfiles
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Where(x => x.OrganisationId == organisationId)
                .Select(a => new
                {
                    Id = a.BusinessProfileId,
                    Property = _context.Properties.Where(c => c.PropertyId == a.PropertyId).FirstOrDefault(),
                    Customer = _context.Customers.Where(c => c.CustomerId == a.CustomerId).FirstOrDefault(),
                    BusinessType = _context.BusinessTypes.Where(p => p.Id == a.BusinessTypeId).FirstOrDefault(),
                    BusinessSize = _context.BusinessSizes.Where(p => p.Id == a.BusinessSizeId).FirstOrDefault(),
                    Revenue = _context.Revenues.Where(p => p.RevenueId == a.RevenueId).FirstOrDefault(),
                    Agency = _context.Agencies.Where(p => p.AgencyId == a.Property.AgencyId).FirstOrDefault(),
                    Organisation = _context.Organisations.Where(p => p.OrganisationId == organisationId).FirstOrDefault()
                }).ToListAsync();

            List<EnumerationManifestDto> result = new List<EnumerationManifestDto>();

            foreach (var group in summaryList.GroupBy(x => new { x.Property.PropertyId, x.Customer.CustomerId }))
            {
                var item = group.First();

                var businessProfiles = group.Select(x => new BusinessProfileDto
                {
                    BusinessType = x.BusinessType?.BusinessTypeName,
                    BusinessSize = x.BusinessSize?.BusinessSizeName,
                    Revenue = x.Revenue?.RevenueName
                }).ToList();

                result.Add(new EnumerationManifestDto
                {
                    Id = item.Id,
                    PropertyName = item.Property?.BuildingName,
                    PropertyAddress = item.Property?.LocationAddress,
                    LastName = item.Customer?.LastName,
                    FirstName = item.Customer?.FirstName,
                    MiddleName = item.Customer?.MiddleName,
                    CustomerAddress = item.Customer?.Address,
                    PayerID = item.Customer?.PayerId,
                    Businessprofile = businessProfiles,
                    Agency = item.Agency?.AgencyName,
                    DateIssued = item.Property.DateCreated,
                    OrganisationName = item.Organisation?.OrganisationName,
                    OrganisationLogo = item.Organisation?.LogoData
                });
            }

            return result;
        }

        public async Task<List<EnumerationManifestDto>> EnumerationManifestById(int organisationId, int id)
        {
            var summaryList = await _context.BusinessProfiles
                .Where(x => x.OrganisationId == organisationId && x.BusinessProfileId == id)
                .Select(a => new
                {
                    Id = a.BusinessProfileId,
                    Property = _context.Properties.Where(c => c.PropertyId == a.PropertyId).FirstOrDefault(),
                    Customer = _context.Customers.Where(c => c.CustomerId == a.CustomerId).FirstOrDefault(),
                    BusinessType = _context.BusinessTypes.Where(p => p.Id == a.BusinessTypeId).FirstOrDefault(),
                    BusinessSize = _context.BusinessSizes.Where(p => p.Id == a.BusinessSizeId).FirstOrDefault(),
                    Revenue = _context.Revenues.Where(p => p.RevenueId == a.RevenueId).FirstOrDefault(),
                    Agency = _context.Agencies.Where(p => p.AgencyId == a.Property.AgencyId).FirstOrDefault(),
                    Organisation = _context.Organisations.Where(p => p.OrganisationId == organisationId).FirstOrDefault()
                }).ToListAsync();

            List<EnumerationManifestDto> result = new List<EnumerationManifestDto>();

            foreach (var group in summaryList.GroupBy(x => new { x.Property.PropertyId, x.Customer.CustomerId }))
            {
                var item = group.First();

                var businessProfiles = group.Select(x => new BusinessProfileDto
                {
                    BusinessType = x.BusinessType?.BusinessTypeName,
                    BusinessSize = x.BusinessSize?.BusinessSizeName,
                    Revenue = x.Revenue?.RevenueName
                }).ToList();

                result.Add(new EnumerationManifestDto
                {
                    Id = item.Id,
                    PropertyName = item.Property?.BuildingName,
                    PropertyAddress = item.Property?.LocationAddress,
                    LastName = item.Customer?.LastName,
                    FirstName = item.Customer?.FirstName,
                    MiddleName = item.Customer?.MiddleName,
                    CustomerAddress = item.Customer?.Address,
                    PayerID = item.Customer?.PayerId,
                    Businessprofile = businessProfiles,
                    Agency = item.Agency?.AgencyName,
                    DateIssued = item.Property.DateCreated,
                    OrganisationName = item.Organisation?.OrganisationName,
                    OrganisationLogo = item.Organisation?.LogoData
                });
            }

            return result;
        }

        public async Task RemoveCustomerFromProperty(int organisationId, int propertyId, int customerId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var customer = await _context.CustomerProperties.Where(x => x.OrganisationId.Equals(organisationId) && x.PropertyId.Equals(propertyId) && x.CustomerId.Equals(customerId)).
                                  Select(x => x).FirstOrDefaultAsync();

            if (customer != null)
            {
                customer.PropertyId = null;
                _context.Entry(customer).Property(x => x.PropertyId).IsModified = true;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<GetCustomerPropertyDto> properties, MetaData metaData)> GetAllCustomerPropertiesAsync(int organisationId, int propertyId,
            DefaultParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);
            await CheckIfPropertyExists(organisationId, propertyId, trackChanges);

            var customerPropertiesWithMetaData = await _repository.CustomerProperty.GetAllCustomersPropertiesAsync(organisationId, propertyId, requestParameters, trackChanges);

            var customerPropertiesDto = _mapper.Map<IEnumerable<GetCustomerPropertyDto>>(customerPropertiesWithMetaData);

            return (properties: customerPropertiesDto, metaData: customerPropertiesWithMetaData.MetaData);
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int organisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(organisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", organisationId);
        }

        private async Task CheckIfPropertyExists(int organisationId, int propertyId, bool trackChanges)
        {
            var property = await _repository.Property.GetPropertyAsync(organisationId, propertyId, trackChanges);
            if (property is null)
                throw new IdNotFoundException("property", propertyId);
        }
    }
}