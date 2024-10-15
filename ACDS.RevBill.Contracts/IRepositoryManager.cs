using System;
namespace ACDS.RevBill.Contracts
{
	public interface IRepositoryManager
	{
		IRoleRepository Roles { get; }
        IUsersRepository Users { get; }
		IUserRoleRepository UserRole { get; }
        IOrganisationRepository Organisation { get; }
        IOrganisationModulesRepository OrganisationModules { get; }
        IUserProfileRepository UserProfile { get; }
        IUserPasswordRepository UserPassword { get; }
        ICustomerRepository Customer { get; }
        ILgasRepository Lgas { get; }
        IMenusRepository Menus { get; }
        IModulesRepository Modules { get; }
        IRoleModulesRepository RoleModules { get; }
        IRoleModuleMenuRepository RoleModuleMenus { get; }
        IEmailTemplateRepository EmailTemplates { get; }
        IEmailAccountRepository EmailAccounts { get; }
        ISmsTemplateRepository SmsTemplates { get; }
        ISmsAccountRepository SmsAccounts { get; }
        IPasswordHistoryRepository PasswordHistory { get; }
        IGenderRepository Gender { get; }
        ITitleRepository Title { get; }
        IMaritalStatusRepository MaritalStatus { get; }
        IPayerTypeRepository PayerType { get; }
        ILcdaRepository Lcdas { get; }
        IStatesRepository States { get; }
        ICountriesRepository Countries { get; }
        IBillingRepository Billing { get; }
        IAgencyRepository Agencies { get; }
        IStreetRepository Streets { get; }
        IRevenueRepository Revenues { get; }
        IRevenueCategoryRepository RevenueCategories { get; }
        ICategoryRepository Category { get; }
        IRevenuePriceRepository RevenuePrices { get; }
        IHeadRevenueRepository HeadRevenues { get; }
        IPropertyRepository Property { get; }
        ISpaceIdentifierRepository SpaceIdentifier { get; }
        IWardRepository Wards { get; }
        IBusinessTypeRepository BusinessType { get; }
        IBusinessSizeRepository BusinessSize { get; }
        IBusinessProfileRepository BusinessProfile { get; }
        ICustomerPropertyRepository CustomerProperty { get; }
        IFrequencyRepository Frequencies { get; }
        ITenancyRepository Tenancy { get; }
        IPaymentRepository Payment { get; }
        IBankRepository Bank { get; }
        IOrganisationBankRepository OrganisationBank { get; }
        IBillFormatRepository BillFormat { get; }
        IDebtRepository debtRepository { get; }

        Task SaveAsync();
    }
}