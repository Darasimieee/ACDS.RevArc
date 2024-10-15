using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Agencies;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Property;
using ACDS.RevBill.Shared.DataTransferObjects.Payment;
using ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.DataTransferObjects.User;
using AutoMapper;

namespace ACDS.RevBill
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //for Roles mapping 
            CreateMap<Roles, RoleDto>();
            CreateMap<RoleForCreationDto, Roles>();
            CreateMap<RoleForUpdateDto, Roles>();

            ////for Department mapping 
            //CreateMap<Department, DepartmentDto>();
            //CreateMap<CreateDepartmentDto, Department>();
            //CreateMap<UpdateDepartmentDto, Department>();

            //For organisation mapping
            CreateMap<Organisation, GetOrganisationDto>();
            CreateMap<CreateOrganisationDto, Organisation>();
            CreateMap<UpdateOrganisationDto, Organisation>();
            CreateMap<Tenancy, GetTenancyDto>();
            CreateMap<CreateTenancyDto, Tenancy>();
            CreateMap<UpdateTenancyDto, Tenancy>();

            //For organisationModule mapping
            CreateMap<OrganisationModules, GetOrganisationModuleDto>();
            CreateMap<CreateOrganisationModuleDto, OrganisationModules>();
            CreateMap<OrganisationModules, CreateOrganisationModuleDto>();
            CreateMap<CreateOrganisationModuleDto, UpdateRoleModuleDto>();
            CreateMap<UpdateRoleModuleDto, OrganisationModules>();

            //For RoleModule mapping
            CreateMap<RoleModules, GetRoleModuleDto>();
            CreateMap<CreateRoleModuleDto, RoleModules>();
            CreateMap<RoleModules, CreateRoleModuleDto>();
            CreateMap<UpdateRoleModuleDto, RoleModules>();

            //For RoleModuleMenu mapping
            CreateMap<RoleModuleMenus, GetRoleModMenuDto>();
            CreateMap<CreateRoleModMenuDto, RoleModuleMenus>();
            CreateMap<RoleModuleMenus, CreateRoleModMenuDto>();
            CreateMap<UpdateRoleModMenuDto, RoleModuleMenus>();

            //For menu mapping
            CreateMap<Menus, GetMenuDto>();
            CreateMap<CreateMenuDto, Menus>();
            CreateMap<UpdateMenuDto, Menus>();

            //For module mapping
            CreateMap<Modules, GetModuleDto>();
            CreateMap<CreateModuleDto, Modules>();
            CreateMap<UpdateModuleDto, Modules>();

            //For EmailTemplate mapping
            CreateMap<EmailTemplates, GetEmailTemplateDto>();
            CreateMap<CreateEmailTemplateDto, EmailTemplates>();
            CreateMap<UpdateEmailTemplateDto, EmailTemplates>();

            //For EmailAccounts mapping
            CreateMap<EmailAccounts, GetEmailAccountDto>();
            CreateMap<CreateEmailAccountDto, EmailAccounts>();
            CreateMap<UpdateEmailAccountDto, EmailAccounts>();

            //For user mapping
            CreateMap<UserCreationDto, Users>();
            CreateMap<UserUpdateDto, Users>();
            CreateMap<UserProfiles, UserProfileDto>();
            CreateMap<UserProfileUpdateDto, UserProfiles>();
            CreateMap<UserPasswords, UserPasswordDto>();
            CreateMap<UserRoles, UserRoleDto>();
            CreateMap<UserRoleUpdateDto, UserRoles>();
            CreateMap<UserProfileCreationDto, Users>();
            CreateMap<UserProfileCreationDto, UserProfiles>();
            CreateMap<UserCreationDto, FullUserDto>();
            CreateMap<UserRequestParams, FullUserDto>();
            CreateMap<UserRoleCreationDto, UserRoles>();
            CreateMap<UserPasswordCreationDto, UserPasswords>();
            CreateMap<CustomerRequestParams, FullUserDto>();
            CreateMap<UserProfiles, FullUserDto>();
            CreateMap<PasswordHistoryCreationDto, PasswordHistory>();
            CreateMap<UserPasswordUpdateDto, UserPasswords>();
            CreateMap<Users, FullUserDto>();
            CreateMap<Users, UserDto>();

            //for customer mapping
            CreateMap<Customers, FullUserDto>();
            CreateMap<Customers, GetCustomerDto>();
            CreateMap<CreateCustomerDto, Customers>();
            CreateMap<UpdateCustomerDto , Customers>();
            CreateMap<Customers, CreateCustomerDto>();

            //Customer Enumeration
            CreateMap<Titles, TitleDto>();
            CreateMap< IEnumerable<Bank_Code>, IEnumerable<GetBankCodeResponse>>();            
            CreateMap<Genders, GenderDto>();
            CreateMap<PayerTypes, PayerTypeDto>();
            CreateMap<MaritalStatuses, MaritalStatusDto>();
            CreateMap<CustomerEnumerationNINDto, Response>();
            CreateMap<CustomerEnumerationBVNDto, Response>();
            CreateMap<Lgas, LgaDto>();

            CreateMap<Lcdas, LcdaDto>()
                .ForMember(u => u.StateId, opt => opt.MapFrom(x => x.CountryId))
                .ForMember(u => u.CountryId, opt => opt.MapFrom(x => x.StateId));
            //state mapping
            CreateMap<States, StateDto>();
            CreateMap<Countries, CountriesDto>();
            CreateMap<PIDResponse, Response>();
            CreateMap<IEnumerable<CreateBusinessProfileMultiRevenueDto>, CreateBusinessProfileDto>();
            CreateMap<IEnumerable<CreateBusinessProfileDto>, BusinessProfile>();
            CreateMap<CreateBusinessProfileDto, BusinessProfile>();
            CreateMap<BusinessProfile, GetBusinessProfileDto>();
            CreateMap<GetTaxPayerByPhoneNumberResponseDto, GetTaxPayerRequestDto>();
            CreateMap<GetTaxPayerByEmailResponseDto, GetTaxPayerRequestDto>();
            CreateMap<CreateCustomerPropertyDto, CustomerProperty>();
            CreateMap<CustomerProperty, GetCustomerPropertyDto>();

            //Agencies mapping
            CreateMap<Agencies, AgencyDto>();
            CreateMap<AgencyCreationDto, Agencies>();
            CreateMap<AgencyUpdateDto, Agencies>();

            //Streets mapping
            CreateMap<Streets, GetStreetDto>();
            CreateMap<IEnumerable<BulkStreetCreation>, StreetCreationDto>();
            CreateMap< StreetCreationDto, Streets>();
            CreateMap<StreetUpdateDto, Streets>();
            CreateMap<UploadCreationDto, Streets>();

            //Revenue mapping
            CreateMap<Revenues, RevenueDto>();
            CreateMap<RevenueCreationDto, Revenues>();
            CreateMap<RevenueUpdateDto, Revenues>();

            //RevenueCategories mapping 
            CreateMap<RevenueCategories, RevenueCategoryDto>();
            CreateMap<RevenueCategoryCreationDto, RevenueCategories>();
            CreateMap<RevenueCategoryUpdateDto, RevenueCategories>();

            //Categories mapping
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreationDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            //RevenuePricing Mapping RevenuePrices
            CreateMap<RevenuePricesDto, RevenuePriceListDto>();
            CreateMap<RevenuePrices, RevenuePricesDto>();
            CreateMap<RevenuePricesCreationDto, RevenuePrices>();
            CreateMap<RevenuePricesUpdateDto, RevenuePrices>();

            //Audit
            CreateMap<AuditReport, AuditTrailDto>();

            //Property Enumeration
            CreateMap<CreatePropertyDto, Property>();
            CreateMap<PropertyUpdateDto, Property>();
            CreateMap<Property, GetPropertiesDto>();
            CreateMap<Property, CreatePropertyDto>();

            ////Wards
            //CreateMap<CreateWardDto, Ward>();
            //CreateMap<UpdateWardDto, Ward>();
            //CreateMap<Ward, GetWardDto>();

            //Space Identifier
            CreateMap<CreateSpaceIdentifierDto, SpaceIdentifier>();
            CreateMap<UpdateSpaceIdentifierDto, SpaceIdentifier>();
            CreateMap<SpaceIdentifier, GetSpaceIdentifierDto>();

            //Business Types
            CreateMap<CreateBusinessTypeDto, BusinessType>();
            CreateMap<UpdateBusinessTypeDto, BusinessType>();
            CreateMap<BusinessType, GetBusinessTypeDto>();

            //Business Sizes
            CreateMap<CreateBusinessSizeDto, BusinessSize>();
            CreateMap<UpdateBusinessSizeDto, BusinessSize>();
            CreateMap<BusinessSize, GetBusinessSizeDto>();

            //Billing
            CreateMap<Frequencies, GetFrequencyDto>();
            CreateMap<IEnumerable<CreatePropertyBill>,CreatePropertyBillDto>();
            CreateMap<CreatePropertyBillDto, Billing>();
            CreateMap<IEnumerable<CreatePropertyBillUpload>, CreatePropertyBillDto>();
            CreateMap<CreatePropertyBillDto, GetBillDto>();
            CreateMap<CreateNonPropertyBillDto, Billing>();
            CreateMap<CreateBacklogBillDto, Billing>();
            CreateMap<Billing, GetBillDto>();
            CreateMap<Billing, GetDebtReportDto>();
            CreateMap<BillStatus, BillStatusDto>();
            CreateMap<BillType, BillTypeDto>();
            CreateMap<IEnumerable<CreatePropertyBillDto>, Billing>();
            CreateMap<IEnumerable<CreateBulkNonProperty>, Billing>();
            CreateMap<IEnumerable<CreateBulkBacklogBill>, Billing>();
            CreateMap<IEnumerable<CreateAutoBill>, Billing>();
            CreateMap<CreateBulkPropertyBill, Billing>();
            CreateMap<CreateBulkNonProperty, Billing>();
            CreateMap<CreateBulkBillingDto, BulkBillingDto>();
            CreateMap<CreateAutoBillDto, Billing>();
            CreateMap<BillFormat, GetBillFormat>();
            CreateMap<CreateBillFormat, BillFormat>();
            CreateMap<UpdateBillFormat, BillFormat>();
            CreateMap<UpdateBilldto, Billing>();
            CreateMap<UpdateBilldto, Updatedto>();
            CreateMap<Updatedto, Billing>();    

            //Payment
            CreateMap<Payment, GetPaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<Banks, GetPaymentGatewayDto>();
            CreateMap<CreatePaymentGatewayDto, Banks>();
            CreateMap<UpdatePaymentGatewayDto, Banks>();
            CreateMap<OrganisationBanks, GetOrganisationPaymentGatewayDto>();
            CreateMap<CreateOrganisationPaymentGatewayDto, OrganisationBanks>();
            CreateMap<UpdateOrganisationPaymentGatewayDto, OrganisationBanks>();
            CreateMap<PaymentbyRevenueDto, Payment>();
            CreateMap<PaymentbyAgencyDto, Payment>();
            CreateMap<PaymentbyBankDto, Payment>();


            //Agency Reporting
            CreateMap<AgencyQuarterlyCollectionResponse,AgencyQuarter2>();
            CreateMap<AgencyQuarterlyCollectionResponse, AgencyQuarter3>();
            CreateMap<AgencyQuarterlyCollectionResponse, AgencyQuarter4>();
            CreateMap<CreateOrganisationPaymentGatewayDto, AgencyBiAnnual2>();
            CreateMap<AgencyBiAnnualCollectionResponse, AgencyBiAnnual2>();

            // Debt Reporting



            CreateMap<CreateOrganisationPaymentGatewayDto, AgencyBiAnnual2>();
        }
    }
}