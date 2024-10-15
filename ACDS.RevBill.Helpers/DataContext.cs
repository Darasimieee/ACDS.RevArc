using System;
using ACDS.RevBill.Entities.Models;
using Audit.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ACDS.RevBill.Helpers
{
	public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _connectionString;

        public DataContext(DbContextOptions options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _connectionString = _httpContextAccessor.HttpContext?.User.Claims
              .FirstOrDefault(claim => claim.Type == "ConnectionString")?.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (_connectionString is not null)
            {
                options.UseSqlServer(_connectionString);
            }

            else
            {
                var configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", false)
                  .Build();

                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
            }
        }

        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Agencies>? Agencies { get; set; }
        public DbSet<Applications>? Applications { get; set; }
        public DbSet<Customers>? Customers { get; set; }
        public DbSet<EmailAccounts>? EmailAccounts { get; set; }
        public DbSet<EmailTemplates>? EmailTemplates { get; set; }
        public DbSet<ModuleComponents>? ModuleComponents { get; set; }
        public DbSet<Modules>? Modules { get; set; }
        public DbSet<Menus>? Menus { get; set; }
        public DbSet<Countries>? Countries { get; set; }
        public DbSet<SpaceIdentifier>? SpaceIdentifiers { get; set; }  
        public DbSet<OrganisationModules>? OrganisationModules { get; set; }
        public DbSet<RoleModules>? RoleModules { get; set; }
        public DbSet<RoleModuleMenus>? RoleModuleMenus { get; set; }
        //public DbSet<OrganisationRoleModules>? OrganisationRoleModules { get; set; }
        public DbSet<Organisation>? Organisations { get; set; }
        public DbSet<PreferenceModes>? PreferenceModes { get; set; }
        public DbSet<Preferences>? Preferences { get; set; }
        public DbSet<Privileges>? Privileges { get; set; }
        public DbSet<RevenueCategories>? RevenueCategories { get; set; }
        public DbSet<RevenuePrices>? RevenuePrices { get; set; }
        public DbSet<Revenues>? Revenues { get; set; }
        public DbSet<SmsAccounts>? SmsAccounts { get; set; }
        public DbSet<SmsTemplates>? SmsTemplates { get; set; }
        public DbSet<UserPasswords>? UserPasswords { get; set; }
        public DbSet<UserProfiles>? UserProfiles { get; set; }
        public DbSet<UserRoles>? UserRoles { get; set; }
        public DbSet<Users>? Users { get; set; }
        public DbSet<EmailTemplateCategory>? EmailTemplateCategory { get; set; }
        public DbSet<UserPasswordResetRequests>? UserPasswordResetRequests { get; set; }
        public DbSet<PasswordHistory>? PasswordHistory { get; set; }
        public DbSet<AccountActivation>? AccountActivation { get; set; }
        public DbSet<Lcdas>? LCDAs { get; set; }
        public DbSet<Lgas>? LocalGovermentAreas { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<AuditTrail>? AuditTrail { get; set; }
        public DbSet<Property>? Properties { get; set; }
        public DbSet<BusinessProfile>? BusinessProfiles { get; set; }
        public DbSet<BillType>? BillType { get; set; }
        public DbSet<Frequencies>? Frequencies { get; set; }
        public DbSet<BusinessType>? BusinessTypes { get; set; }
        public DbSet<BusinessSize>? BusinessSizes { get; set; }
        public DbSet<Billing>? Billing { get; set; }
        public DbSet<CustomerProperty>? CustomerProperties { get; set; }
        public DbSet<Tenancy>? Tenancy { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Banks>? Banks { get; set; }
        public DbSet<OrganisationBanks>? OrganisationBanks { get; set; }
       public DbSet<HeadRevenue>? HeadRevenue { get; set; }
        public DbSet<BillFormat>? BillFormats { get; set; }
        public DbSet<Bank_Code>? Bank_Code { get; set; }
        //public DbSet<Ward>? Wards { get; set; }
        public DbSet<Streets>? Streets { get; set; }
        public DbSet<Department>? Departments { get; set; }
    }
}