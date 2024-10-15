using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Repository.Extensions
{
    public static class RepositoryBillingExtensions
    {
        public static IQueryable<Billing> SearchByPayerID(this IQueryable<Billing> bills, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return bills;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return bills.Where(e => e.Customers.PayerId.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Billing> SearchByPayerTypeId(this IQueryable<Billing> bills, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return bills;
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return bills.Where(e => e.Customers.PayerTypeId.Equals(lowerCaseTerm));
        }
        public static IQueryable<Billing> SearchByRevenue(this IQueryable<Billing> bills, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return bills;
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return bills.Where(e => e.Revenues.RevenueName.Equals(lowerCaseTerm));
        }
        public static IQueryable<Billing> SearchByCustomerName(this IQueryable<Billing> bills, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return bills;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return bills.Where(e => e.Customers.FullName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Billing> SearchByAreaOffice(this IQueryable<Billing> bills, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return bills;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return bills.Where(e => e.Agencies.AgencyName.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Billing> SearchByState(this IQueryable<Billing> bills, int? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm.ToString())|| searchTerm.Equals(0))
                return bills;            

            return bills.Where(e => e.Organisations.StateId.Equals(searchTerm));
        }
        public static IQueryable<Billing> SearchByLga(this IQueryable<Billing> bills, int? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm.ToString()))
                return bills;

            return bills.Where(e => e.Organisations.LgaId.Equals(searchTerm));

        }
        public static IQueryable<Billing> SearchByLcda(this IQueryable<Billing> bills, int? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm.ToString()))
                return bills;

            return bills.Where(e => e.Organisations.LcdaId.Equals(searchTerm));
        }
    }
}