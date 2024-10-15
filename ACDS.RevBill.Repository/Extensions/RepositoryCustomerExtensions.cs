using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Repository.Extensions
{
    public static class RepositoryCustomerExtensions
    {
        public static IQueryable<Customers> SearchByName(this IQueryable<Customers> customers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return customers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return customers.Where(e => e.FullName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Customers> SearchByEmail(this IQueryable<Customers> customers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return customers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return customers.Where(e => e.Email.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Customers> SearchByPhoneNumber(this IQueryable<Customers> customers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return customers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return customers.Where(e => e.PhoneNo.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Customers> SearchByPayerID(this IQueryable<Customers> customers, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return customers;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return customers.Where(e => e.PayerId.ToLower().Contains(lowerCaseTerm));
        }
    }
}

