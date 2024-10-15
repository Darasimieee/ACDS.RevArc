using ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency;

namespace ACDS.RevBill.Service.Contracts
{
	public interface ITransactionReportBuilderService
	{
        Task<string> ColllectionByAgency(int organisationId);
        Task<string> ColllectionByRevenue(int organisationId);
        Task<string> DailyCollection(int organisationId);
        Task<string> AgencyMonthlyCollection(int organisationId);
        Task<IEnumerable<AgencyYearlyCollectionResponse>> AgencyYearlyCollection(AgencyYearlyCollectionRequest agencyCollections);
        Task<IEnumerable<object>> AgencyQuarterlyCollection(AgencyQuarterlyCollectionRequest agencyQuarterlyCollections);
        Task<IEnumerable<object>> GetAgencyBiAnnualCollection(AgencyBiAnnualCollectionRequest bianualCol);
        Task<string> TotalPropertyCount(int organisationId);
        Task<string> TotalCustomerInPropertyCount(int organisationId);
        Task<string> TotalNonPropertyCount(int organisationId);
        Task<string> TransactionCount(int organisationId);
        Task<string> TotalCustomerCount(int organisationId);
    }
}