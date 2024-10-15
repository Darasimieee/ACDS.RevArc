using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using ACDS.RevBill.Repository.Extensions;

namespace ACDS.RevBill.Repository
{
    internal sealed class PropertyRepository : RepositoryBase<Property>, IPropertyRepository
    {
        public PropertyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        //GetPropertiesbyAgencyAsync
        public async Task<PagedList<Property>> GetPropertiesbyAgencyAsync(int OrganisationId, int agencyId, PropertyParameters requestParameters, bool trackChanges)
        {
            //var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            var properties = FindAll(false);

            //var properties = await FindByCondition(e => e.OrganisationId.Equals(OrganisationId) && e.AgencyId.Equals(agencyId) &&
            //    (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate) &&
            //    (requestParameters.SpaceIdentifier == null || e.SpaceIdentifier.SpaceIdentifierName == requestParameters.SpaceIdentifier), trackChanges)
            //   .SearchByBuilidingName(requestParameters.Building)
            //   .SearchByLocationAddress(requestParameters.Address)
            //   .OrderBy(e => e.PropertyId)
            //   .Include(o => o.SpaceIdentifier)
            //   .Include(o => o.Agencies)
            //   .ToListAsync();

            return PagedList<Property>
                .ToPagedList(properties, requestParameters.PageNumber, requestParameters.PageSize);
        }
        public async Task<PagedList<Property>> GetAllPropertiesAsync(int OrganisationId, PropertyParameters requestParameters, bool trackChanges)
        {

            var endDate = requestParameters.EndDate == DateTime.MaxValue ? requestParameters.EndDate : requestParameters.EndDate.AddDays(1);

            //var properties = FindAll(trackChanges);

            try
            {
                var properties = await FindByCondition(e => e.OrganisationId==OrganisationId &&
                     (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate) &&
                     (requestParameters.SpaceIdentifier == null ||
                     e.SpaceIdentifier.SpaceIdentifierName == requestParameters.SpaceIdentifier), trackChanges)
                    .SearchByBuilidingName(requestParameters.Building)
                    .SearchByLocationAddress(requestParameters.Address)
                   // .OrderByDescending(e => e.PropertyId)
                   .Include(o => o.SpaceIdentifier)
                    .Include(o => o.Agencies)
                    .ToListAsync();

                return PagedList<Property>
                    .ToPagedList(properties, requestParameters.PageNumber, requestParameters.PageSize);
            }
            catch (InvalidCastException iex)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        //public async Task<PagedList<Property>> GetAllPropertiesAsync(int organisationId, PropertyParameters requestParameters, bool trackChanges)
        //{
        //    // Adjust endDate calculation for clarity and correctness
        //    //DateTime endDate = requestParameters.EndDate == DateTime.MaxValue ? DateTime.MaxValue : requestParameters.EndDate.AddDays(1);
        //    var properties = FindAll(false);
        //    //var properties = await FindByCondition(e => e.OrganisationId == organisationId &&
        //    //                                             (e.DateCreated >= requestParameters.StartDate && e.DateCreated <= endDate) &&
        //    //                                             (requestParameters.SpaceIdentifier == null || e.SpaceIdentifier.SpaceIdentifierName == requestParameters.SpaceIdentifier),
        //    //                                       trackChanges)
        //    //                       .SearchByLocationAddress(requestParameters.Address)
        //    //                       .OrderByDescending(e => e.PropertyId)
        //    //                       .Include(o => o.SpaceIdentifier)
        //    //                       .Include(o => o.Agencies)
        //    //                       .ToListAsync();

        //    // Convert List<Property> to PagedList<Property>
        //    return PagedList<Property>.ToPagedList(properties, requestParameters.PageNumber, requestParameters.PageSize);
        //}


        public async Task<Property> GetPropertyAsync(int OrganisationId, int PropertyId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(OrganisationId) && c.PropertyId.Equals(PropertyId), trackChanges)
           .Include(o => o.SpaceIdentifier)
           .Include(o => o.Agencies)
           .SingleOrDefaultAsync();


        public void CreateProperty(int OrganisationId, Property property)
        {
            property.OrganisationId = OrganisationId;
            Create(property);
        }
    }
}