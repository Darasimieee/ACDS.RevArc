using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IBillingService
    {
        Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetAllBillsAsync(int organisationId, BillingParameters requestParameters, bool trackChanges);
        Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetPayerBillsAsync(int organisationId, int agencyId, string payerId, BillingParameters requestParameters, bool trackChanges);
        Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetAgencyBillsAsync(int organisationId, int agencyId, BillingParameters requestParameters, bool trackChanges);
        Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> FilterBillstobePaid(int organisationId, DebtReportParameters requestParameters, bool trackChanges);
        Task<GetBillDto> GetBillByBillIdAsync(int organisationId, int billId, bool trackChanges);
        Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetBillByCustomerIdAsync(int organisationId, int customerId, DefaultParameters requestParameters, bool trackChanges);
        Task<IEnumerable<GetBillDto>> BillByCustomerIdHarmonisedIdAsync(int organisationId, int customerId, string harmonisedbillref);
        Task<Response> CreatePropertyBillAsync(int organisationId, int propertyId, int customerId, CreateBulkPropertyBill createBillDto, bool trackChanges);
        Task<IEnumerable<GetBillDto>> CreateNonPropertyBillAsync(int organisationId, int customerId, CreateBulkNonProperty createBillDto, bool trackChanges);
        Task<IEnumerable<GetBillDto>> BackLogBill(int organisationId, int propertyId, int customerId, CreateBulkBacklogBill createBillDto, bool trackChanges);
        Task<IEnumerable<GetBillDto>> AutoBillGeneration(int organisationId, CreateAutoBill createBillDto, bool trackChanges);
        Task<IEnumerable<GetBillDto>> BulkBilling(int organisationId, CreateBulkBillingDto bulkBilling, bool trackChanges);
        Task<string> NoOfBillsGeneratedThisMonth(int organisationId);
        Task<string> NoOfBillsGeneratedThisWeek(int organisationId);
        Task<string> NoOfBillsGeneratedToday(int organisationId);
        Task<string> TotalAmountOfBillsGeneratedThisMonth(int organisationId);
        Task<string> TotalAmountOfBillsGeneratedThisWeek(int organisationId);
        Task<string> TotalAmountOfBillsGeneratedToday(int organisationId);
        Task<DebtReportDto> TotalBilltobePaidToday(int organisationId);
        Task<DebtReportDto> TotalBilltobePaidThisMonth(int organisationId);
        Task<DebtReportDto> TotalBilltobePaidThisYear(int organisationId);
        Task<List<AgencyBillingSummaryDto>> NoOfBillsByAreaOffice(int organisationId);
        Task<IEnumerable<GetFrequencyDto>> GetAllFrequency(bool trackChanges);
        Task<List<BillingReportDto>> GenerateBillReport(int organisationId, long billId);
        Task<List<BillingReportDto>> GenerateBillReport(int organisationId, string harmonized);
        Task<ValidateBillResponseDto> ValidateBill(ValidateBillRequest1Dto validateBill);
        Response ValidateHarmonizedBillReferences(HarmonizedBillReferenceRequestDto harmonizedBill);
        Task<Response> StepDownBill(int organisationId, int billId, StepDownBillDto stepDown);
        Task<string> CountOfSteppedDownBills(int organisationId, int billId);
        Task<Response> CreateBillFormatAsync(int organisationId, CreateBillFormat createBillFormat, bool trackChanges);
        Task<Response> UpdateBillFormatAsync(int organisationId, int billFormatId, UpdateBillFormat updateBillFormat, bool trackChanges);
        Task<Response> UpdateBillAsync(int organisationId, int propertyId, int customerId,UpdatePropertyBill updateBill, bool trackChanges);
        Task<(IEnumerable<GetBillFormat> bills, MetaData metaData)> GetAllBillFormatsAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<GetBillFormat> GetBillFormatAsync(int organisationId, int billFormatId, bool trackChanges);
        Task<Response> UploadBillsAsync(int organisationId,string creator, DefaultParameters requestParameters, IFormFile file);
        Task<List<PreviewedbillResponse>> BulkPreviewedBilling(int organisationId,string createdby, IEnumerable<CreatePropertyBillUpload> previewedbill, bool trackChanges);
    }
}