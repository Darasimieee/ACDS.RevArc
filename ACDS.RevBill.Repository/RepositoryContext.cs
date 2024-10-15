using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Repository.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ACDS.RevBill.Repository
{
    public class RepositoryContext : DbContext
    {
        private string? _tenantId;
        private string? _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RepositoryContext(DbContextOptions<RepositoryContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantId = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(claim => claim.Type == "TenantId")?.Value;
            _connectionString = _httpContextAccessor.HttpContext?.User.Claims
              .FirstOrDefault(claim => claim.Type == "ConnectionString")?.Value;

            if (_connectionString is not null)
            {
                Database.SetConnectionString(_connectionString);
            }

            else
            {                
                var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false)
               .Build();

                Database.SetConnectionString(configuration.GetConnectionString("sqlConnection"));
            }      
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EmailTemplateCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new GenderConfiguration());
            modelBuilder.ApplyConfiguration(new MaritalStatusConfiguration());
            modelBuilder.ApplyConfiguration(new TitleConfiguration());
            modelBuilder.ApplyConfiguration(new PayerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpaceIdentifierConfiguration());
           // modelBuilder.ApplyConfiguration(new WardConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessSizeConfiguration());
            modelBuilder.ApplyConfiguration(new BillStatusConfiguration());
            modelBuilder.ApplyConfiguration(new FrequencyConfiguration()); 
            modelBuilder.ApplyConfiguration(new BillTypeConfiguration());

            base.OnModelCreating(modelBuilder);          
        }

        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Agencies>? Agencies { get; set; }
        public DbSet<Streets>? Streets { get; set; }
        public DbSet<Applications>? Applications { get; set; }
        public DbSet<Customers>? Customers { get; set; }
        public DbSet<EmailAccounts>? EmailAccounts { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<EmailTemplates>? EmailTemplates { get; set; }
        public DbSet<ModuleComponents>? ModuleComponents { get; set; }
        public DbSet<Modules>? Modules { get; set; }
        public DbSet<Menus>? Menus { get; set; }
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
        public DbSet<Lgas>? LocalGovermentAreas { get; set; }
        public DbSet<States>? States { get; set; }
        public DbSet<Lcdas>? LCDAs { get; set; }
        public DbSet<Countries>? Countries { get; set; }
        public DbSet<EmailTemplateCategory>? EmailTemplateCategory { get; set; }
        public DbSet<UserPasswordResetRequests>? UserPasswordResetRequests { get; set; }
        public DbSet<PasswordHistory>? PasswordHistory { get; set; }
        public DbSet<AccountActivation>? AccountActivation { get; set; }
        public DbSet<OrganisationModules>? OrganisationModules { get; set; }
        public DbSet<RoleModules>? RoleModules { get; set; }
        public DbSet<RoleModuleMenus>? RoleModuleMenus { get; set; }
        public DbSet<Property>? Properties { get; set; }
        public DbSet<SpaceIdentifier>? SpaceIdentifiers { get; set; }
       // public DbSet<Ward>? Wards { get; set; }
        public DbSet<BusinessType>? BusinessTypes { get; set; }
        public DbSet<BusinessSize>? BusinessSizes { get; set; }
        public DbSet<BusinessProfile>? BusinessProfiles { get; set; }
        public DbSet<AuditTrail>? AuditTrail { get; set; }
        public DbSet<Billing>? Billing { get; set; }
        public DbSet<CustomerProperty>? CustomerProperties { get; set; }
        public DbSet<Tenancy>? Tenancy { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Banks>? Banks { get; set; }
        public DbSet<OrganisationBanks>? OrganisationBanks { get; set; }
        public DbSet<HeadRevenue>? HeadRevenue { get; set; }
        public DbSet<BillFormat>? BillFormats { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantName = _tenantId;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}