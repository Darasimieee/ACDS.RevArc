using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Payment;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface IPaymentRepository
	{
        Task<PagedList<Payment>> GetAllPaymentsAsync(int organisationId, PaymentParameters requestParameters, bool trackChanges);
        Task<Payment> GetPaymentAsync(int organisationId, long paymentId, bool trackChanges);
        void CreatePayment(Payment payment);
    }
}