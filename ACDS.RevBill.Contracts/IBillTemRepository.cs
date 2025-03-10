using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
    public interface IBillTemRepository
    {
        Task<BillPreApproval> GetBillAsync(long billpreapprovalId, bool trackChanges);

        void CreatePropertyBill(int organisationId, int propertyId, int customerId, IEnumerable<BillPreApproval> billings);
    }
}