using System;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class BillFormatRepository : RepositoryBase<BillFormat>, IBillFormatRepository
    {
        public BillFormatRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<BillFormat>> GetAllBillFormatsAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            var bills = await FindByCondition(e => e.OrganisationId.Equals(organisationId), trackChanges)
                 .OrderBy(e => e.BillFormatId)
                 .ToListAsync();

            return PagedList<BillFormat>
                .ToPagedList(bills, requestParameters.PageNumber, requestParameters.PageSize);
        }

        public async Task<BillFormat> GetBillFormatAsync(int organisationId, int billFormatId, bool trackChanges) =>
            await FindByCondition(c => c.OrganisationId.Equals(organisationId) && c.BillFormatId.Equals(billFormatId), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateBillFormat(int organisationId, BillFormat billFormat)
        {
            billFormat.OrganisationId = organisationId;
            Create(billFormat);
        }
    }
}