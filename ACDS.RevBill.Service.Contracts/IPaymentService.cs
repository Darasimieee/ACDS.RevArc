using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Payment;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IPaymentService
	{
        Task<(IEnumerable<GetPaymentDto> payment, MetaData metaData)> GetOrganisationPaymentsAsync(int organisationId, PaymentParameters requestParameters, bool trackChanges);
        Task<List<PaymentByAgency>> GetOrganisationPaymentByAgencyAsync(int organisationId);

        Task<List<PaymentbyRevenueDto>> GetOrganisationPaymentByRevenueAsync(int organisationId);
        Task<List<PaymentbyBankDto>> GetOrganisationPaymentByBankAsync(int organisationId);
        Task<List<DailyPaymentbyBankDto>> GetOrganisationDailyPaymentByBankAsync(int organisationId);
        Task<GetPaymentDto> GetPaymentInOrganisationAsync(int organisationId, long paymentId, bool trackChanges);
        Task<Response> GetAllIndividualPaymentHistoriesAsync(int userId);
        Task<Response> GetIndividualPaymentHistoryAsync(int userId, long paymentId);
        Task<Response> AddPaymentHistoryAsync(CreatePaymentDto payment);
        Task<(IEnumerable<GetPaymentGatewayDto> banks, MetaData metaData)> GetAllPaymentGatewaysAsync(DefaultParameters requestParameters, bool trackChanges);
        Task<GetPaymentGatewayDto> GetPaymentGatewayAsync(int id, bool trackChanges);
        Task<Response> AddPaymentGatewayAsync(CreatePaymentGatewayDto bank);
        Task<Response> UpdatePaymentGatewayAsync(int id, UpdatePaymentGatewayDto bank, bool trackChanges);
        Task<(IEnumerable<GetOrganisationPaymentGatewayDto> banks, MetaData metaData)> GetAllOrganisationPaymentGatewaysAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<GetOrganisationPaymentGatewayDto> GetOrganisationPaymentGatewayAsync(int organisationid, int organisationBankId, bool trackChanges);
        Task<Response> AddPaymentGatewayToOrganisationAsync(int organisationId, IEnumerable<CreateOrganisationPaymentGatewayDto> bank);
        Task<Response> UpdatePaymentGatewayForOrganisationAsync(int organisationId, int paymentGatewayId, UpdateOrganisationPaymentGatewayDto bank, bool trackChanges);
        Task<Response> TotalPaymentsThisYear(int organisationId);
        Task<Response> TotalPaymentsThisMonth(int organisationId);
        Task<Response> TotalPaymentsThisWeek(int organisationId);
        Task<Response> TotalPaymentsToday(int organisationId);
        Task<Response> TotalCountOfPaymentsThisMonth(int organisationId);
        Task<Response> TotalCountOfPaymentsThisWeek(int organisationId);
        Task<Response> TotalCountOfPaymentsToday(int organisationId);
    }
}