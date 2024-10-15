using System;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Service.Contracts
{
	public interface IAuditTrailService
	{
        Task<IEnumerable<AuditTrailDto>> GetAuditTrailAsync(PaginationFilter filter);
    }
}

