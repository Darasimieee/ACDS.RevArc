using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using ACDS.RevBill.Repository.Extensions;

namespace ACDS.RevBill.Repository
{
    internal sealed class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Payment>> GetAllPaymentsAsync(int organisationId, PaymentParameters requestParameters, bool trackChanges)
        {
            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            var payments = await FindByCondition(x => true &&
                (x.EntryDate >= requestParameters.StartDate && x.EntryDate <= endDate), trackChanges)
                .SearchByRevenue(requestParameters.Revenue)
                .SearchByAgency(requestParameters.Agency)
                .SearchByPayerID(requestParameters.PayerId)
                .SearchByBankCode(requestParameters.BankCode)
                .ToListAsync();

            // Perform filtering in memory using AsEnumerable and IndexOf
            payments = payments.AsEnumerable()
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero)
                .OrderBy(x => x.PaymentId)
                .ToList();

            return PagedList<Payment>
                .ToPagedList(payments, requestParameters.PageNumber, requestParameters.PageSize);
        }
    
        public async Task<Payment> GetPaymentAsync(int organisationId, long paymentId, bool trackChanges)
        {
            string organisationIdString = organisationId.ToString();
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var payments = await FindByCondition(x => true, trackChanges).ToListAsync();

            // Perform filtering in memory using AsEnumerable and IndexOf
            var payment = payments.AsEnumerable()
                .FirstOrDefault(x =>
                    x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero &&
                    x.PaymentId == paymentId);

            return payment;
        }

        public void CreatePayment(Payment payment)
        {
            Create(payment);
        }
    }
}