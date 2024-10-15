using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Repository.Extensions;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ACDS.RevBill.Repository
{
    internal sealed class BillingRepository : RepositoryBase<Billing>, IBillingRepository
    {
        public BillingRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Billing>> GetAllBillsAsync(int organisationId, BillingParameters requestParameters, bool trackChanges)
        {
            var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId)
           && (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate)
            , trackChanges)


                 .OrderByDescending(e => e.BillId)
                 .SearchByPayerID(requestParameters.PayerId)
                 .SearchByCustomerName(requestParameters.CustomerName)
                 .SearchByAreaOffice(requestParameters.AreaOffice)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Frequencies)
                 .Include(o => o.BillType)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .Include(o => o.BusinessSize)
                 .ToListAsync();

            return PagedList<Billing>
                .ToPagedList(bills, requestParameters.PageNumber, requestParameters.PageSize);
        }
        public async Task<PagedList<Billing>> GetAgencyBillsAsync(int organisationId,int agencyId, BillingParameters requestParameters, bool trackChanges)
        {
            var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId) && e.AgencyId.Equals(agencyId) &&
                (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate), trackChanges)
                 .OrderByDescending(e => e.BillId)
                 .SearchByPayerID(requestParameters.PayerId)
                 .SearchByCustomerName(requestParameters.CustomerName)
                 .SearchByAreaOffice(requestParameters.AreaOffice)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Frequencies)
                 .Include(o => o.BillType)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .Include(o => o.BusinessSize)
                 .ToListAsync();

            return PagedList<Billing>
                .ToPagedList(bills, requestParameters.PageNumber, requestParameters.PageSize);
        }
        public async Task<PagedList<Billing>> GetPayerBillsAsync(int organisationId, int agencyId, BillingParameters requestParameters, bool trackChanges)
        {
            var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId) && e.AgencyId.Equals(agencyId) &&
                (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate), trackChanges)
                 .OrderByDescending(e => e.BillId)
                 .SearchByPayerID(requestParameters.PayerId)
                 .SearchByCustomerName(requestParameters.CustomerName)
                 .SearchByAreaOffice(requestParameters.AreaOffice)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Frequencies)
                 .Include(o => o.BillType)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .Include(o => o.BusinessSize)
                 .ToListAsync();

            return PagedList<Billing>
                .ToPagedList(bills, requestParameters.PageNumber, requestParameters.PageSize);
        }
        public async Task<PagedList<Billing>> GetAllBillsAsync(int organisationId, DebtReportParameters requestParameters, bool trackChanges)
        {
            var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId) &&
                (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate), trackChanges)
                 .OrderBy(e => e.BillId)
                 .SearchByPayerID(requestParameters.PayerId)
                 .SearchByPayerTypeId(requestParameters.PayerTypeId)                 
                 .SearchByAreaOffice(requestParameters.AreaOffice)
                 .SearchByRevenue(requestParameters.Revenue)
                 .SearchByLcda(requestParameters.LcdaId)
                 .SearchByLga(requestParameters.LgaId)
                 .SearchByState(requestParameters.stateId)
                 .Include(o => o.Organisations)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .ToListAsync();

            return PagedList<Billing>
                .ToPagedList(bills, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<Billing> GetBillAsync(int organisationId, long billId, bool trackChanges) =>        
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.BillId.Equals(billId), trackChanges)
             .Include(o => o.Customers)
             .Include(o => o.Agencies)
             .Include(o => o.Revenues)
             .Include(o => o.BillStatus)
             .Include(o => o.Frequencies)
             .Include(o => o.BillType)
             .Include(o => o.Property)
             .Include(o => o.BusinessType)
             .Include(o => o.BusinessSize)
             .SingleOrDefaultAsync();

        public async Task<IEnumerable<Billing>> GetCustomerBillbyYearAsync(int organisationId, int customerId, int propertyId, bool trackChanges)
        {
            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId) && e.CustomerId.Equals(customerId)&&e.PropertyId==propertyId && e.BillTypeId == 1 && e.Year == DateTime.Now.Year, trackChanges)
                 .OrderBy(e => e.BillId)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Frequencies)
                 .Include(o => o.BillType)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .Include(o => o.BusinessSize)
                 .ToListAsync();
            return bills;

        }

        public async Task<PagedList<Billing>> GetCustomerBillAsync(int organisationId, int customerId, DefaultParameters requestParameters, bool trackChanges)
        {
            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId) && e.CustomerId.Equals(customerId), trackChanges)
                 .OrderBy(e => e.BillId)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Frequencies)
                 .Include(o => o.BillType)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .Include(o => o.BusinessSize)
                 .ToListAsync();
            return PagedList<Billing>
                .ToPagedList(bills, requestParameters.PageNumber, requestParameters.PageSize);
            
        }

        public async Task<IEnumerable<Billing>> GetCustomerBillHarmonisedIdAsync(int organisationId, int customerId,string harmonisedbillref)
        {
            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId) && e.CustomerId.Equals(customerId) && e.HarmonizedBillReferenceNo.Equals(harmonisedbillref), false)
                 .OrderBy(e => e.BillId)
                 .Include(o => o.Customers)
                 .Include(o => o.Agencies)
                 .Include(o => o.Revenues)
                 .Include(o => o.BillStatus)
                 .Include(o => o.Frequencies)
                 .Include(o => o.BillType)
                 .Include(o => o.Property)
                 .Include(o => o.BusinessType)
                 .Include(o => o.BusinessSize)
                 .ToListAsync();
            return bills;

        }
        public void CreatePropertyBill(int organisationId, int propertyId, int customerId, IEnumerable<Billing> billings)
        {
            foreach(var billing in billings)
            {
                billing.OrganisationId = organisationId;
                billing.CustomerId = customerId;
                billing.PropertyId = propertyId;
            }

            CreateMultiple(billings);
        }
       
        public void CreateBill(int organisationId, int propertyId, int customerId, Billing billing)
        {
      
                billing.OrganisationId = organisationId;
                billing.CustomerId = customerId;
                billing.PropertyId = propertyId;
                billing.CreatedBy= billing.ModifiedBy;
                billing.DateModified=billing.DateModified;
        

            Create(billing);
        }

        public void CreateNonPropertyBill(int organisationId, int customerId, IEnumerable<Billing> billings)
        {
            foreach (var billing in billings)
            {
                billing.OrganisationId = organisationId;
                billing.CustomerId = customerId;
            }

            CreateMultiple(billings);
        }

        public void CreateAutoGeneratedBill(int organisationId, int propertyId, int customerId, Billing billings)
        {            
            billings.OrganisationId = organisationId;
            billings.CustomerId = customerId;
            billings.PropertyId = propertyId;

            Create(billings);
        }
    }
}