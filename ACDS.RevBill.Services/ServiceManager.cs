using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ACDS.RevBill.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IRoleService> _roleService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IOrganisationService> _organisationService;
        private readonly Lazy<ICustomerService> _customerService;
        private readonly Lazy<IMenuService> _menuService;
        private readonly Lazy<IModuleService> _moduleService;
        private readonly Lazy<IOrganisationModuleService> _orgmoduleService;
        private readonly Lazy<IRoleModuleservice> _orgRoleModuleService;
        private readonly Lazy<IRoleModuleMenuService> _orgRoleModuleMenuService;
        private readonly Lazy<IEmailAccountService> _emailAccountService;
        private readonly Lazy<IEmailTemplateService> _emailTemplateService;
        private readonly Lazy<IEnumerationService> _enumerationService;
        private readonly Lazy<IAuditTrailService> _auditTrailService;
        private readonly Lazy<ISmsAccountService> _smsAccountService;
        private readonly Lazy<ISmsTemplateService> _smsTemplateService;
        private readonly Lazy<IAgencyService> _agencyService;
        private readonly Lazy<IStreetService> _streetService;
        private readonly Lazy<IRevenueService> _revenueService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IRevenueCategoryService> _revenueCategoryService;
        private readonly Lazy<IRevenuePriceService> _revenuePriceService;
        private readonly Lazy<IBillingService> _billingService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<ITransactionReportBuilderService> _transactionReportBuilderService;
        private readonly Lazy<IDebtService> _debtService;
        private readonly Lazy<IWebHostEnvironment> _webHostEnvironment;
        private readonly ILoggerManager _loggerManager;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, ILoggerManager loggerManager,
            IMailService mailService, DataContext context, PID pidConfig, JsonModelService modelService, IWebHostEnvironment webHostEnvironment,  IConfiguration configuration)
        {
            _roleService = new Lazy<IRoleService>(() =>
                new RoleService(repositoryManager, logger, mapper));

            _userService = new Lazy<IUserService>(() =>
                new UserService(repositoryManager, logger, mapper, mailService, context));

            _organisationService = new Lazy<IOrganisationService>(() =>
                new OrganisationService(repositoryManager, logger, mapper, mailService, context, modelService, configuration));

            _customerService = new Lazy<ICustomerService>(() =>
              new CustomerService(repositoryManager, logger, mapper, mailService, context));

            _menuService = new Lazy<IMenuService>(() =>
             new MenuService(repositoryManager, logger, mapper));

            _moduleService = new Lazy<IModuleService>(() =>
            new ModuleService(repositoryManager, logger, mapper));

            _orgmoduleService = new Lazy<IOrganisationModuleService>(() =>
            new OrganisationModuleService(repositoryManager, logger, mapper, context));

            _orgRoleModuleService = new Lazy<IRoleModuleservice>(() =>
           new RoleModuleService(repositoryManager, logger, mapper, context));
            _orgRoleModuleMenuService = new Lazy<IRoleModuleMenuService>(() =>
         new RoleModuleMenuService(repositoryManager, logger, mapper, context));
             _emailAccountService = new Lazy<IEmailAccountService>(() =>
            new EmailAccountService(repositoryManager, logger, mapper));

            _emailTemplateService = new Lazy<IEmailTemplateService>(() =>
            new EmailTemplateService(repositoryManager, logger, mapper));

            _enumerationService = new Lazy<IEnumerationService>(() =>
            new EnumerationService(repositoryManager, logger, loggerManager, mapper, context, pidConfig, modelService));

            _auditTrailService = new Lazy<IAuditTrailService>(() =>
           new AuditTrailService(logger, mapper, context));

            _smsAccountService = new Lazy<ISmsAccountService>(() =>
           new SmsAccountService(repositoryManager, logger, mapper));

            _smsTemplateService = new Lazy<ISmsTemplateService>(() =>
            new SmsTemplateService(repositoryManager, logger, mapper));

            _agencyService = new Lazy<IAgencyService>(() => 
            new AgencyService(repositoryManager, logger, mapper,context, modelService, pidConfig));
            _streetService = new Lazy<IStreetService>(() =>
            new StreetService(repositoryManager, logger, mapper, context, webHostEnvironment, pidConfig));

            _revenueService = new Lazy<IRevenueService>(() =>
             new RevenueService(repositoryManager, logger, mapper, context, modelService, pidConfig));

            _categoryService = new Lazy<ICategoryService>(() =>
            new CategoryService(repositoryManager, logger, mapper));

            _revenueCategoryService = new Lazy<IRevenueCategoryService>(() =>
            new RevenueCategoryService(repositoryManager, logger, mapper));

            _revenuePriceService = new Lazy<IRevenuePriceService>(() =>
            new RevenuePriceService(repositoryManager, logger, context, mapper));

            _billingService = new Lazy<IBillingService>(() =>
           new BillingService(repositoryManager, logger, mapper, modelService, context, pidConfig, mailService, webHostEnvironment));

            _paymentService = new Lazy<IPaymentService>(() =>
           new PaymentService(repositoryManager, logger, mapper, context));

           _transactionReportBuilderService = new Lazy<ITransactionReportBuilderService>(() =>
           new TransactionReportBuilderService(repositoryManager, logger, mapper, modelService, context));
            _debtService = new Lazy<IDebtService>(() => new DebtService());
            
        }

        public IRoleService RoleService => _roleService.Value;
        public IUserService UserService => _userService.Value;
        public IOrganisationService OrganisationService => _organisationService.Value;
        public ICustomerService CustomerService => _customerService.Value;
        public IMenuService MenuService => _menuService.Value;
        public IModuleService ModuleService => _moduleService.Value;
        public IOrganisationModuleService OrganisationModuleService => _orgmoduleService.Value;
        public IRoleModuleservice RoleModuleservice => _orgRoleModuleService.Value;
        public IRoleModuleMenuService RoleModuleMenuService => _orgRoleModuleMenuService.Value;
        public IEmailAccountService EmailAccountService => _emailAccountService.Value;
        public IEmailTemplateService EmailTemplateService => _emailTemplateService.Value;
        public IEnumerationService EnumerationService => _enumerationService.Value;
        public IAuditTrailService AuditTrailService => _auditTrailService.Value;
        public ISmsAccountService SmsAccountService => _smsAccountService.Value;
        public ISmsTemplateService SmsTemplateService => _smsTemplateService.Value;
        public IAgencyService AgencyService => _agencyService.Value;
        public IStreetService StreetService => _streetService.Value;
        public IRevenueService RevenueService => _revenueService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
        public IRevenueCategoryService RevenueCategoryService => _revenueCategoryService.Value;
        public IRevenuePriceService RevenuePriceService => _revenuePriceService.Value;
        public IBillingService BillingService => _billingService.Value;
        public IPaymentService PaymentService => _paymentService.Value;
        public ITransactionReportBuilderService TransactionReportBuilderService => _transactionReportBuilderService.Value;
        public IDebtService DebtService => _debtService.Value;
    }
}