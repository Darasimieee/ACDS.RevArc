using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public record GetBillDto(int BillId, int OrganisationId, GetPropertiesDto Property , GetCustomerDto Customers, string BillReferenceNo, string? HarmonizedBillReferenceNo, AgencyDto Agencies, RevenueDto Revenues,
        decimal BillAmount, decimal AmountPaid, decimal BillArrears,decimal Billbf,GetBusinessTypeDto BusinessType, BillStatusDto BillStatus, GetFrequencyDto Frequencies, BillTypeDto BillType, string AppliedDate, string Category);
}

