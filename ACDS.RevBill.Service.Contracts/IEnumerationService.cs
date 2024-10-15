using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Property;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IEnumerationService
    {
        Task<IEnumerable<TitleDto>> GetAllTitlesAsync(bool trackChanges);
        Task<IEnumerable<GenderDto>> GetAllGendersAsync(bool trackChanges);
        Task<IEnumerable<PayerTypeDto>> GetAllPayerTypesAsync(bool trackChanges);
        Task<IEnumerable<MaritalStatusDto>> GetAllMaritalStatusAsync(bool trackChanges);
        Task<Response> PushBankcodeAsync();
        Task<IEnumerable<Bank_Code>> GetAllBanksAsync();
        Task<IEnumerable<LgaDto>> GetAllLgasAsync(bool trackChanges);
        Task<IEnumerable<LgaDto>> GetLgasByStateAsync(int stateId, bool trackChanges);
        Task<IEnumerable<LcdaDto>> GetAllLcdasAsync(bool trackChanges);
        Task<IEnumerable<LcdaDto>> GetLcdasByLgaAsync(int lgaId, bool trackChanges);
        Task<IEnumerable<StateDto>> GetAllStatesAsync(bool trackChanges);
        Task<IEnumerable<CountriesDto>> GetAllCountriesAsync(bool trackChanges);
        Task<Response> CreatePIDWithNIN(CustomerEnumerationNINDto customer);
        Task<Response> CreatePIDWithBVN(CustomerEnumerationBVNDto customer);
        Task<Response> CreatePIDWithBioData(CustomerEnumerationDto customer);
        Response CreateCorporatePID(CorporatePayerIDRequest customer);
        Response VerifyPid(PayerIdEnumerationDto customer);
        Task<(IEnumerable<GetPropertiesDto> properties, MetaData metaData)> GetAllPropertiesAsync(int organisationId, PropertyParameters requestParameters, bool trackChanges);
        Task<(IEnumerable<GetPropertiesDto> properties, MetaData metaData)> GetPropertiesbyAgencyAsync(int organisationId,int agencyId, PropertyParameters requestParameters, bool trackChanges);
        Task<GetPropertiesDto> GetPropertyAsync(int organisationId, int propertyId, bool trackChanges);
        Task<Response> CreatePropertyAsync(int organisationId, CreatePropertyDto createProperty);
        Task UpdatePropertyAsync(int organisationId, int propertyId, PropertyUpdateDto propertyUpdate, bool trackChanges);
        Task<(IEnumerable<GetBusinessProfileDto> businessProfile, MetaData metaData)> GetAllBusinessProfilesAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<GetBusinessProfileDto> GetBusinessProfileAsync(int organisationId, int businessProfileId, bool trackChanges);
        Task<Response> EnumerationAsync(int organisationId, CompleteEnumerationParams enumeration, bool trackChanges);
        Task<Response> EnumerationWhenPropertyExistsAsync(int organisationId, int propertyId, PartialEnumerationParams enumeration, bool trackChanges);
        Response PayerIdSearchByPhoneNumber(GetTaxPayerRequestDto getTaxPayer);
        Response PayerIdSearchByName(GetTaxPayerRequestDto getTaxPayer);
        Response PayerIdSearchByEmail(GetTaxPayerRequestDto getTaxPayer);
        Task<string> NoOfRegisteredPropertiesThisMonth(int organisationId);
        Task<string> NoOfRegisteredPropertiesThisWeek(int organisationId);
        Task<string> NoOfRegisteredPropertiesToday(int organisationId);
        
        Task<string> NoOfRegisteredNonPropertiesThisMonth(int organisationId);
        Task<string> NoOfRegisteredNonPropertiesThisWeek(int organisationId);
        Task<string> NoOfRegisteredNonPropertiesToday(int organisationId);
        Task<string> NoOfRegisteredCustomersThisMonth(int organisationId);
        Task<string> NoOfRegisteredCustomersThisWeek(int organisationId);
        Task<string> NoOfRegisteredCustomersToday(int organisationId);
        Task<List<AgencySummaryDto>> NoOfPropertiesAndCustomersByAreaOffice(int organisationId);
        Task<List<EnumerationManifestDto>> EnumerationManifest(int organisationId, PaginationFilter filter);
        Task<List<EnumerationManifestDto>> EnumerationManifestById(int organisationId, int id);
        Task RemoveCustomerFromProperty(int organisationId, int propertyId, int customerId, bool trackChanges);
        Task<(IEnumerable<GetCustomerPropertyDto> properties, MetaData metaData)> GetAllCustomerPropertiesAsync(int organisationId, int propertyId, DefaultParameters requestParameters, bool trackChanges);

        ////Wards
        //Task<IEnumerable<GetWardDto>> GetAllWardsAsync(int organisationId, bool trackChanges);
        //Task<GetWardDto> GetWardAsync(int organisationId, int wardId, bool trackChanges);
        //Task<GetWardDto> CreateWardAsync(int organisationId, CreateWardDto createWard);
        //Task UpdateWardAsync(int organisationId, int wardId, UpdateWardDto updateWard, bool trackChanges);

        //Space Identifier
        Task<IEnumerable<GetSpaceIdentifierDto>> GetAllSpaceIdentifiersAsync(int organisationId, bool trackChanges);
        Task<GetSpaceIdentifierDto> GetSpaceIdentifierAsync(int organisationId, int spaceIdentifierId, bool trackChanges);
        Task<GetSpaceIdentifierDto> CreateSpaceIdentifierAsync(int organisationId, CreateSpaceIdentifierDto createSpaceIdentifier);
        Task UpdateSpaceIdentifierAsync(int organisationId, int spaceIdentifierId, UpdateSpaceIdentifierDto updateSpaceIdentifier, bool trackChanges);

        //Business Type
        Task<IEnumerable<GetBusinessTypeDto>> GetAllBusinessTypesAsync(int organisationId, bool trackChanges);
        Task<GetBusinessTypeDto> GetBusinessTypeAsync(int organisationId, int businessTypeId, bool trackChanges);
        Task<GetBusinessTypeDto> CreateBusinessTypeAsync(int organisationId, CreateBusinessTypeDto createBusinessType);
        Task UpdateBusinessTypeAsync(int organisationId, int businessTypeId, UpdateBusinessTypeDto updateBusinessType, bool trackChanges);
        Response VerifyAgencyCode(string agencycode);
        //Business Size
        Task<IEnumerable<GetBusinessSizeDto>> GetAllBusinessSizesAsync(int organisationId, bool trackChanges);
        Task<GetBusinessSizeDto> GetBusinessSizeAsync(int organisationId, int businessSizeId, bool trackChanges);
        Task<GetBusinessSizeDto> CreateBusinessSizeAsync(int organisationId, CreateBusinessSizeDto createBusinessSize);
        Task UpdateBusinessSizeAsync(int organisationId, int businessSizeId, UpdateBusinessSizeDto updateBusinessSize, bool trackChanges);
   
       
    }
}