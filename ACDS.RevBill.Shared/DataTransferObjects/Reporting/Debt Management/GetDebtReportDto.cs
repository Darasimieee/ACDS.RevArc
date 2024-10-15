using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
    public record GetDebtReportDto(int BillId, int OrganisationId, GetPropertiesDto Property, GetCustomerDto Customers, string BillReferenceNo, string? HarmonizedBillReferenceNo, AgencyDto Agencies, RevenueDto Revenues, decimal BillAmount,
        decimal AmountPaid, decimal BillArrears, BillStatusDto BillStatus, GetFrequencyDto Frequencies, BillTypeDto BillType, string AppliedDate, string Category);
}
