using ACDS.RevBill.Entities.Models;
using System.Linq;

namespace ACDS.RevBill.Repository.Extensions
{
	public static class RepositoryPaymentExtension
	{
        public static IQueryable<Payment> SearchByPayerID(this IQueryable<Payment> payments, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return payments;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return payments.Where(e => e.PayerId.ToString().ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Payment> SearchByAgency(this IQueryable<Payment> payments, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return payments;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return payments.Where(e => e.Agency.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Payment> SearchByRevenue(this IQueryable<Payment> payments, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return payments;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return payments.Where(e => e.Revenue.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Payment> SearchByBankCode(this IQueryable<Payment> payments, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return payments;

            // var lowerCaseTerm = searchTerm.Trim().ToLower();

            return payments.Where(e => e.BankCode.Equals(searchTerm));
        }
    }
}