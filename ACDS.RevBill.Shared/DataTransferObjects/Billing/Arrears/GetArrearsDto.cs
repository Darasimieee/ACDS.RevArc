using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public record GetArrearsDTO(int ArrearId, int OrganisationId, string? Year, int Percentage, bool ArrearsApplicable, bool InterestApplicable, bool Active, Organisation organisation);
}
