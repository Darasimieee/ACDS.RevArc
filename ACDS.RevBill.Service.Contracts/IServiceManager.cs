using System;
namespace ACDS.RevBill.Service.Contracts
{
	public interface IServiceManager
	{
		IRoleService RoleService { get; }
		IUserService UserService { get; }
        IOrganisationService OrganisationService { get; }
		ICustomerService CustomerService { get; }
        IMenuService MenuService { get; }
        IModuleService ModuleService { get; }
        IOrganisationModuleService OrganisationModuleService { get; }
        IRoleModuleservice RoleModuleservice { get; }
        IRoleModuleMenuService RoleModuleMenuService { get; }
        IEmailAccountService EmailAccountService { get; }
        IEmailTemplateService EmailTemplateService { get; }
        IEnumerationService EnumerationService { get; }
        IAuditTrailService AuditTrailService { get; }
        ISmsAccountService SmsAccountService { get; }
        ISmsTemplateService SmsTemplateService { get; }
        IAgencyService AgencyService { get; }
        IStreetService StreetService { get; }
        IRevenueService RevenueService { get; }
        ICategoryService CategoryService { get; }
        IRevenueCategoryService RevenueCategoryService { get; }
        IRevenuePriceService RevenuePriceService { get; }
        IBillingService BillingService { get; }
        IPaymentService PaymentService { get; }
        ITransactionReportBuilderService TransactionReportBuilderService { get; }
        IDebtService DebtService { get; }
	
    }
}