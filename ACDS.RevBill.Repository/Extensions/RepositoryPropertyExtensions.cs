using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Repository.Extensions
{
	public static class RepositoryPropertyExtensions
	{
        public static IQueryable<Property> SearchByBuilidingName(this IQueryable<Property> properties, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return properties;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return properties.Where(e => e.BuildingName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Property> SearchByLocationAddress(this IQueryable<Property> properties, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return properties;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return properties.Where(e => e.LocationAddress.ToLower().Contains(lowerCaseTerm));
        }
    }
}