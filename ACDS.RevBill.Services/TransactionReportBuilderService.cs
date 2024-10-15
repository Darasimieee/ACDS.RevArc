using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Services
{
    internal sealed class TransactionReportBuilderService : ITransactionReportBuilderService
    {

        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private DataContext _context;
        public JsonModelService _modelService;
        public TransactionReportBuilderService(IRepositoryManager repository,ILoggerManager logger, IMapper mapper, JsonModelService modelService, DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _modelService= modelService; 
            _context = context;
        }

        public Task<string> ColllectionByAgency(int organisationId)
        {
            throw new NotImplementedException();
        }

        public Task<string> ColllectionByRevenue(int organisationId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DailyCollection(int organisationId)
        {
            throw new NotImplementedException();
        }

        public Task<string> AgencyMonthlyCollection(int organisationId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> TotalCustomerCount(int organisationId)
        {
            int countOfCustomers = await _context.Customers.Where(x => x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfCustomers.ToString("N0");

            return formattedNum;
        }

        public async Task<string> TotalPropertyCount(int organisationId)
        {
            int countOfRegisteredNonProperties = await _context.Billing.Where(x => x.OrganisationId == organisationId && x.PropertyId == null).CountAsync();
            string formattedNum = countOfRegisteredNonProperties.ToString("N0");

            return formattedNum;
        }
        public async Task<string> TotalCustomerInPropertyCount(int organisationId)
        {
            var propertywithCustomers = await _context.CustomerProperties.Where(x => x.OrganisationId == organisationId).GroupBy(x => x.PropertyId).ToListAsync();

            int countOfRegisteredProperties = propertywithCustomers.Count();
             string formattedNum = countOfRegisteredProperties.ToString("N0");

            return formattedNum;
        }
        public async Task<string> TotalNonPropertyCount(int organisationId)
        {
            int countOfRegisteredNonProperties = await _context.Billing.Where(x => x.OrganisationId == organisationId && x.PropertyId == null).CountAsync();

            string formattedNum = countOfRegisteredNonProperties.ToString("N0");

            return formattedNum;
        }
      
        public Task<string> TransactionCount(int organisationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AgencyYearlyCollectionResponse>> AgencyYearlyCollection(AgencyYearlyCollectionRequest agencyCol)
        {
            ////get the agency 
            //var getagencyref = _context.Agencies.Where(x => x.AgencyId == agencyCol.AgencyId).FirstOrDefault();
                            
            //if (getagencyref == null)
            //    throw new IdNotFoundException("Agency", agencyCol.AgencyId);

            //agencyCol.AgencyRef = getagencyref.AgencyCode;
              //call the procedure
              var response = await _modelService.GetAgencyYearlyCollections(agencyCol);
            
            return response;
        }
        public async Task<IEnumerable<object>> AgencyQuarterlyCollection( AgencyQuarterlyCollectionRequest agencyColQuarterly)
        {
            ////get the agency 
            //var getagencyref = _context.Agencies.Where(x => x.AgencyId == agencyColQuarterly.AgencyId).FirstOrDefault();

            //if (getagencyref == null)
            //    throw new IdNotFoundException("Agency", agencyColQuarterly.AgencyId);

            //agencyColQuarterly.AgencyRef = getagencyref.AgencyCode;
            //call the procedure
            var response =  _modelService.GetAgencyQuarterlyCollections(agencyColQuarterly);
        
            return response.Result;
        }
        public async Task<IEnumerable<object>> GetAgencyBiAnnualCollection(AgencyBiAnnualCollectionRequest bianualCol)
        {
            ////get the agency 
            //var getagencyref = _context.Agencies.Where(x => x.AgencyId == bianualCol.AgencyId).FirstOrDefault();

            //if (getagencyref == null)
            //    throw new IdNotFoundException("Agency", bianualCol.AgencyId);

            //bianualCol.AgencyRef = getagencyref.AgencyCode;
            ////call the procedure
            var response = _modelService.GetAgencyBiAnnualCollection(bianualCol);

            return response.Result;
        }
    }
}