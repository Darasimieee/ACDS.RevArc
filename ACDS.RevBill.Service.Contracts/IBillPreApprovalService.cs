using ACDS.RevBill.Entities;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IBillPreApprovalService
    {
        Task<List<Response>> CreatePropertyBillAsync(int organisationId, int propertyId, int customerId, CreateBulkPropertyBill createBillDto, bool trackChanges);
    }
}
