using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Email;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using AutoMapper.Internal;
using MathNet.Numerics.Random;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Math.EC;
using SkiaSharp;
using SkiaSharp.QrCode;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Property = ACDS.RevBill.Entities.Models.Property;

namespace ACDS.RevBill.Services
{
    internal sealed class BillingService : IBillingService
    {
        //private const string SingleBillPrefix = "SB"; 
       //private const string HarmonizedBillPrefix = "HB"; 


        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public JsonModelService _modelService;
        private DataContext _context;
        private readonly PID _pidConfig;
        private IMailService _mailService;
        public IEnumerable<GenerateBillResponse>? BillResponses;
        public IEnumerable<UpdateBillResponse>? updateResponses;
        public IEnumerable<HarmonizedBillReferenceResponseDto>? GetHarmonizedBills;
        private readonly int _maxFileSize = 200000; //in bytes
        private readonly string[] _extensions = { ".jpg", ".png", ".jpeg" };
        public string? LogoFileName { get; set; }
        public string? LogoFileExtension { get; set; }
        public string? LogoNewFileName { get; set; }
        public string? LogoFileName1 { get; set; }
        public string? LogoFileExtension1 { get; set; }
        public string? LogoNewFileName1 { get; set; }
        private string? body { get; set; }
        public IEnumerable<VerifyPidResponseDto>? VerifyPidResponse;
        public IEnumerable<GetTaxPayerByPhoneNumberResponseDto>? GetTaxPayerByPhoneNumber;
        public IEnumerable<GetTaxPayerByNameResponseDto>? GetTaxPayerByName;
        public IEnumerable<GetTaxPayerByEmailResponseDto>? GetTaxPayerByEmail;
        public IEnumerable<CorporatePayerIDResponse>? GenerateCorporatePID;
        //public IEnumerationService _enumerationService;
        //public ICustomerService _customerService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public BillingService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, JsonModelService modelService,
             DataContext context, PID pidConfig, IMailService mailServic, IWebHostEnvironment webHostEnvironment)
        //,IEnumerationService enumerationService, ICustomerService customerService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _modelService = modelService;
            _context = context;
            _pidConfig = pidConfig;
            _mailService = mailServic;
            _webHostEnvironment = webHostEnvironment;
            //_enumerationService = enumerationService;
            //_customerService = customerService;
        }

        public async Task<IEnumerable<GetFrequencyDto>> GetAllFrequency(bool trackChanges)
        {
            var frequency = await _repository.Frequencies.GetAllFrequencyAsync(trackChanges);

            var frequencyDto = _mapper.Map<IEnumerable<GetFrequencyDto>>(frequency);

            return frequencyDto;
        }

        public async Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetPayerBillsAsync(int organisationId, int agencyId, string payerId, BillingParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            if (!requestParameters.ValidDateRange)
                throw new MaxDateRangeBadRequestException();
            requestParameters.PayerId = payerId;
            requestParameters.StartDate = DateTime.Now.AddYears(-3);
            var billsWithMetaData = await _repository.Billing.GetPayerBillsAsync(organisationId, agencyId, requestParameters, trackChanges);

            var billsDto = _mapper.Map<IEnumerable<GetBillDto>>(billsWithMetaData);

            return (bills: billsDto, metaData: billsWithMetaData.MetaData);
        }


        public async Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetAgencyBillsAsync(int organisationId, int agencyId, BillingParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            if (!requestParameters.ValidDateRange)
                throw new MaxDateRangeBadRequestException();

            var billsWithMetaData = await _repository.Billing.GetAgencyBillsAsync(organisationId, agencyId, requestParameters, trackChanges);

            var billsDto = _mapper.Map<IEnumerable<GetBillDto>>(billsWithMetaData);

            return (bills: billsDto, metaData: billsWithMetaData.MetaData);
        }


        public async Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetAllBillsAsync(int organisationId, BillingParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            if (!requestParameters.ValidDateRange)
                throw new MaxDateRangeBadRequestException();

            var billsWithMetaData = await _repository.Billing.GetAllBillsAsync(organisationId, requestParameters, trackChanges);

            var billsDto = _mapper.Map<IEnumerable<GetBillDto>>(billsWithMetaData);

            return (bills: billsDto, metaData: billsWithMetaData.MetaData);
        }

        public async Task<GetBillDto> GetBillByBillIdAsync(int organisationId, int billId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var bill = await _repository.Billing.GetBillAsync(organisationId, billId, trackChanges);
            //check if the bill is null
            if (bill is null)
                throw new IdNotFoundException("bill", billId);

            var billDto = _mapper.Map<GetBillDto>(bill);

            return billDto;
        }

        public async Task<IEnumerable<GetBillDto>> BillByCustomerIdHarmonisedIdAsync(int organisationId, int customerId, string harmonisedbillref)
        {
            await CheckIfOrganisationExists(organisationId, false);

            var billsWithMetaData = await _repository.Billing.GetCustomerBillHarmonisedIdAsync(organisationId, customerId, harmonisedbillref);

            var billsDto = _mapper.Map<IEnumerable<GetBillDto>>(billsWithMetaData);

            return billsDto;
        }
        public async Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> GetBillByCustomerIdAsync(int organisationId, int customerId, DefaultParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billsWithMetaData = await _repository.Billing.GetCustomerBillAsync(organisationId, customerId, requestParameters, trackChanges);

            var billsDto = _mapper.Map<IEnumerable<GetBillDto>>(billsWithMetaData);

            return (bills: billsDto, metaData: billsWithMetaData.MetaData);
        }

        public async Task<Response> CreatePropertyBillAsync(int organisationId, int propertyId, int customerId,
   CreateBulkPropertyBill createBillDto, bool trackChanges)
        {
            Response response = new();
            await CheckIfOrganisationExists(organisationId, trackChanges);
            var originalPayerID = await _context.Customers
                .Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId))
                .FirstOrDefaultAsync();

            if (originalPayerID == null)
            {
                response.Status = 404;
                response.StatusMessage = "Customer does not exist!";
                response.Data = customerId;
                return response;
            }

            var customerbills = await CheckIfBillExists(organisationId, propertyId, customerId);
            var customerproperty = await CheckPropertyCustomer(customerId, propertyId);

            // Checking if customer is registered in this property; if not, register customer in property
            if (customerproperty == 0)
            {
                CreateCustomerProperty(customerId, propertyId, organisationId, createBillDto.CreatePropertyBillDto.First().CreatedBy);
            }

            List<CreatePropertyBillDto> billexist = new List<CreatePropertyBillDto>();
            List<CreatePropertyBillDto> bills = new List<CreatePropertyBillDto>();

            foreach (CreatePropertyBill item in createBillDto.CreatePropertyBillDto)
            {
                foreach (BillRevenuePricesDto a in item.BillRevenuePrices)
                {
                    CreatePropertyBillDto billDto = new CreatePropertyBillDto
                    {
                        BusinessTypeId = item.BusinessTypeId,
                        BusinessSizeId = item.BusinessSizeId,
                        AgencyId = item.AgencyId,
                        AppliedDate = item.AppliedDate,
                        DateCreated = item.DateCreated,
                        CreatedBy = item.CreatedBy,
                        RevenueId = a.RevenueId,
                        Category = a.Category,
                        BillAmount = a.BillAmount
                    };

                    var checkBillbytype = customerbills
                        .Where(x => x.BusinessTypeId == item.BusinessTypeId && x.RevenueId == a.RevenueId && x.AppliedDate == item.AppliedDate)
                        .Count();

                    if (checkBillbytype != 0)
                    {
                        billexist.Add(billDto);
                    }
                    else
                    {
                        bills.Add(billDto);
                    }
                }
            }

            var billEntity = _mapper.Map<IEnumerable<Billing>>(bills);
            var billToReturn = _mapper.Map<IEnumerable<GetBillDto>>(billEntity);
            var payerID = originalPayerID.PayerId.Substring(2);
            var billcount = billEntity.Count();
            string harmonizedBillReference = string.Empty;

           

            if (billcount >= 1 && customerbills.Select(e => e.HarmonizeStore).FirstOrDefault() == null)
            {
                harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReference(payerID, organisationId);
                harmonizedBillReference = "HB-" + harmonizedBillReference; // Prefix for harmonized bill
                Console.WriteLine($"Generated Harmonized Bill Reference: {harmonizedBillReference}"); // Debug output
            }
            else
            {
                harmonizedBillReference = customerbills.Select(e => e.HarmonizeStore).FirstOrDefault();
                Console.WriteLine($"Retrieved Harmonized Bill Reference: {harmonizedBillReference}"); // Debug output
            }

            if (customerbills.Count == 1)
            {
                // Update bills without harmonized reference
                foreach (var item in customerbills)
                {
                    item.HarmonizedBillReferenceNo = item.HarmonizeStore;
                    item.ModifiedBy = billEntity.Select(e => e.ModifiedBy).Single();
                    item.DateModified = DateTime.Now;
                    _context.Billing.Update(item);
                    _context.SaveChanges();
                }
            }

            foreach (var record in billEntity)
            {
                // Get agency and revenue codes
                var agencyCode = await _context.Agencies
                    .Where(x => x.AgencyId.Equals(record.AgencyId) && x.OrganisationId.Equals(organisationId))
                    .Select(x => x.AgencyCode).FirstOrDefaultAsync();

                var revenueCode = await _context.Revenues
                    .Where(x => x.RevenueId.Equals(record.RevenueId) && x.OrganisationId.Equals(organisationId))
                    .FirstOrDefaultAsync();

                var propertyAddress = await _context.Properties
                    .Where(x => x.PropertyId.Equals(propertyId) && x.OrganisationId.Equals(organisationId))
                    .Select(x => x.BuildingNo + ", " + x.LocationAddress)
                    .FirstOrDefaultAsync();

                // Get current date and future date for applied date
                DateTime currentDate = DateTime.Now;
                string formattedDateTime = currentDate.ToString("yyyy-MM-dd");
                string currentYear = currentDate.Year.ToString();

                // Generate bill reference with prefix
                record.BillReferenceNo = "SB-" + BillingUtility.GenerateBillReference(payerID, organisationId); // Prefix for single bill
                record.HarmonizedBillReferenceNo = harmonizedBillReference;

                GenerateBillRequest billRequest = new()
                {
                    Amount = record.BillAmount,
                    PayerID = originalPayerID.PayerId,
                    AgencyRef = agencyCode,
                    RevCode = revenueCode.RevenueCode,
                    EntryDate = formattedDateTime,
                    AppliedDate = record.AppliedDate,
                    BillReference = record.BillReferenceNo,
                    HarmonizedBillReference = record.HarmonizedBillReferenceNo,
                    AssessRef = record.BillReferenceNo,
                    Year = currentYear,
                    PropertyAddress = propertyAddress
                };

                // Call procedure to push bill to EBS-RCM
                BillResponses = _modelService.GenerateBillReference(billRequest);

                // As requested by business, a single bill does not need harmonized ref 
                // Not storing the harmonized bill reference for a single bill in its field
                // though saving it in EBS and a nullable field called HarmonizeStore
                // If ever another bill is generated, I then update harmonized bill reference No field
                // with what is in HarmonizeStore
                if (billcount == 1 && customerbills.Count == 0)
                {
                    record.HarmonizeStore = harmonizedBillReference;
                    record.HarmonizedBillReferenceNo = null;
                }

                record.BillStatusId = 1;
                record.BillTypeId = 1;
                record.FrequencyId = 6;
                record.BillArrears = record.BillAmount;
                record.AmountPaid = 0;
                record.Billbf = 0;
                record.Year = DateTime.Now.Year;
            }

            if (billexist.Count > 0)
            {
                response.StatusMessage = "Bill(s) Exist! " + harmonizedBillReference;
                response.Data = billexist;
                response.Status = 200;
            }
            else
            {
                _repository.Billing.CreatePropertyBill(organisationId, propertyId, customerId, billEntity);
                await _repository.SaveAsync();

                // Add customer to the property
                if (billcount == 1)
                {
                    var billId = _context.Billing
                        .Where(x => x.BillReferenceNo == billEntity.FirstOrDefault().BillReferenceNo)
                        .FirstOrDefault().BillId;

                    response.StatusMessage = "Bill generated successfully";
                    response.Data = billId;
                    response.Status = 200;
                }
                else
                {
                    response.StatusMessage = "Bill generated successfully";
                    response.Data = harmonizedBillReference;
                    response.Status = 200;
                }

                // Send mail to customer
                MailRequest mailRequest = new MailRequest
                {
                    Subject = "Billing",
                    ToEmail = originalPayerID.Email,
                    Body = Getbody(billEntity),
                    FirstName = originalPayerID.FirstName,
                    LastName = originalPayerID.LastName
                };
                // await _mailService.SendBillGenerationAsync(mailRequest);
            }

            return response;
        }
        public string Getbody(IEnumerable<Billing> bills)
        {
            int count = 1;
            int num = bills.Count(); // Assuming bills is a collection
            decimal total = 0;
            string tableRows = "";
            if (num > 1)
            {
                tableRows = "<div><table border=\"1\" cellspacing=\"0\" cellpadding=\"5\" style=\"border-collapse: collapse; width: 100%;\">\r\n<thead>\r\n<tr><th>Revenue Name</th><th >Bill Amount</th><th >Total</th></tr></thead><tbody>";

            }
            else
            {
                tableRows = "<div><table border=\"1\" cellspacing=\"0\" cellpadding=\"5\" style=\"border-collapse: collapse; width: 100%;\">\r\n<thead>\r\n<tr><th >Bill Reference No</th><th>Revenue Name</th><th >Bill Amount</th></tr></thead><tbody>";

            }
            foreach (var item in bills)
            {
                total += item.BillAmount;
                if (num > 1)
                {
                    tableRows += "<tr><td style=\"padding-left: 10px;\" >" +
                             (_context.Revenues.FirstOrDefault(x => x.RevenueId == item.RevenueId)?.RevenueName ?? "") + "</td><td style=\"padding-left: 10px;\" >" +
                             item.BillAmount.ToString("#,##0.00") + "</td><td style=\"padding-left: 10px;\" >";
                    if (count == num)
                    {
                        tableRows += total;
                    }
                }
                else
                {
                    tableRows += "<tr><td style=\"padding-left: 10px;\" >" + item.AppliedDate + "</td><td style=\"padding-left: 10px;\" >" + item.BillReferenceNo + "</td><td style=\"padding-left: 10px;\" >" +
                                                (_context.Revenues.FirstOrDefault(x => x.RevenueId == item.RevenueId)?.RevenueName ?? "") + "</td><td style=\"padding-left: 10px;\" >" +
                                                item.BillAmount.ToString("#,##0.00") + "</td><td style=\"padding-left: 10px;\" >";
                }


                tableRows += "</td></tr>";

                count++;
            }
            if (num > 1)
            {
                tableRows += "</tbody></table></div><div><br /><p><strong> Harmonized Reference : " + bills.First().HarmonizedBillReferenceNo + "</strong></p></div> ";

            }
            else
            {
                tableRows += "</tbody></table> ";

            }
            total.ToString("#,##0.00");
            var bill = bills.FirstOrDefault();
            if(bill != null)
            {
                tableRows += " <p> Beneficiary : <strong>" + (_context.Organisations.FirstOrDefault(x => x.OrganisationId == bills.First().OrganisationId)?.OrganisationName ?? "") + " </strong></p> <p> Applied Date : <strong>" + bills.First().AppliedDate;
                tableRows += GetOrgPaymentBanks(bills.First().OrganisationId);

                

            }
            return tableRows;

        }
        public string GetOrgPaymentBanks(int organisationId)
        {
            var thebanks = "<strong/> You can pay the invoice through any of the collecting banks below: <br> ";
            var banks = (from orgBank in _context.OrganisationBanks.Where(x => x.OrganisationId == organisationId)
                         join bank in _context.Banks on orgBank.BankId equals bank.BankId
                         select new
                         {
                             // Select the properties you need from both tables
                             OrganisationBankId = orgBank.OrganisationBankId,
                             OrganisationId = orgBank.OrganisationId,
                             BankId = bank.BankId,
                             BankName = bank.BankName,
                         }).ToList();
            foreach (var bank in banks)
            {
                thebanks += "<p style=\"color:royalblue;\">" + bank.BankName + "</p>";
            }
            return thebanks;
        }
        public async Task<Response> UpdateBillAsync(int organisationId, int propertyId, int customerId, UpdatePropertyBill updateBill, bool trackChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackChanges);

            var originalPayerID = await _context.Customers.Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId)).Select(x => x.PayerId).FirstOrDefaultAsync();

            var payerID = originalPayerID.Substring(2);

            string harmonizedBillReference = updateBill.HarmonizedBillReferenceNo;

            var billEntity = _mapper.Map<IEnumerable<Billing>>(updateBill.UpdateBilldto);

            if (updateBill.HarmonizedBillReferenceNo == null)
            {
                harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReference(payerID, organisationId);
            }

            var billToReturn = _mapper.Map<IEnumerable<GetBillDto>>(billEntity);


            foreach (var record in billEntity)
            {
                if (record.BillId != 0)
                {
                    Updatedto updatedto = new Updatedto();
                    var editbill = updateBill.UpdateBilldto.Where(x => x.BillId == record.BillId).Single();

                    _mapper.Map(editbill, updatedto);

                    var bill = _repository.Billing.GetBillAsync(organisationId, record.BillId, true);

                    UpdateBillRequest updateRequest = new()
                    {
                        Amount = bill.Result.BillAmount,
                        Editedby = bill.Result.ModifiedBy,
                        BillReference = bill.Result.BillReferenceNo
                    };
                    //get payerId
                    var editorId = _context.Users.SingleOrDefault(u => (u.Email == record.ModifiedBy)).TenantName;
                    //call procedure to push bill to EBS-RCMTe


                    updateResponses = _modelService.UpdateBillonEbs(Convert.ToInt32(editorId.Substring(2)), updateRequest);
                    if (updateResponses.First().Outputdata == "ok update")
                    {
                        _mapper.Map(updatedto, bill.Result);
                        dataResponse.StatusMessage = "Bill  Upgrade Successfully";
                        await _repository.SaveAsync();
                    }
                    else
                    {
                        dataResponse.StatusMessage = "Bill  Upgrade partially Successful old bill(s) not updated";
                    }

                }
                else
                {
                    var isExist = _context.Billing.Where(x => x.AmountPaid.Equals(record.AmountPaid) && x.Category.Equals(record.Category) && x.RevenueId.Equals(record.RevenueId) && x.BusinessTypeId.Equals(record.BusinessTypeId) && x.BusinessSizeId.Equals(record.BusinessSizeId) && x.HarmonizedBillReferenceNo.Equals(harmonizedBillReference) && x.AppliedDate.Equals(record.AppliedDate)).Single();
                    if (isExist != null)
                    {
                        dataResponse.StatusMessage = "Bill  Upgrade Successfully but bill with category: " + record.Category + "Exists";
                    }
                    var agencyCode = await _context.Agencies.Where(x => x.AgencyId.Equals(record.AgencyId) && x.OrganisationId.Equals(organisationId))
                                   .Select(x => x.AgencyCode).FirstOrDefaultAsync();

                    var revenueCode = await _context.Revenues.Where(x => x.RevenueId.Equals(record.RevenueId) && x.OrganisationId.Equals(organisationId))
                                        .Select(x => x.RevenueCode).FirstOrDefaultAsync();

                    var propertyAddress = await _context.Properties.Where(x => x.PropertyId.Equals(propertyId) && x.OrganisationId.Equals(organisationId))
                                            .Select(x => x.BuildingNo + ", " + x.LocationAddress)
                                            .FirstOrDefaultAsync();

                    //get current date and future date for applied date
                    DateTime currentDate = DateTime.Now;
                    string formattedDateTime = currentDate.ToString("yyyy-MM-dd");
                    string currentYear = currentDate.Year.ToString();

                    //Generate bill reference
                    record.BillReferenceNo = BillingUtility.GenerateBillReference(payerID, organisationId);

                    GenerateBillRequest billRequest = new()
                    {
                        Amount = record.BillAmount,
                        PayerID = originalPayerID,
                        AgencyRef = agencyCode,
                        RevCode = revenueCode,
                        EntryDate = formattedDateTime,
                        AppliedDate = record.AppliedDate,
                        BillReference = record.BillReferenceNo,
                        HarmonizedBillReference = harmonizedBillReference,
                        AssessRef = record.BillReferenceNo,
                        Year = currentYear,
                        PropertyAddress = propertyAddress
                    };

                    //call procedure to push bill to EBS-RCM
                    BillResponses = _modelService.GenerateBillReference(billRequest);

                    record.BillStatusId = 1;
                    record.BillTypeId = 1;
                    record.FrequencyId = 6;
                    record.BillArrears = record.BillAmount;
                    record.AmountPaid = 0;
                    record.Billbf = 0;
                    record.Year = DateTime.Now.Year;
                    _repository.Billing.CreateBill(organisationId, propertyId, customerId, record);

                    await _repository.SaveAsync();
                }


            }

            dataResponse.Status = 200;
            dataResponse.Data = billEntity;

            return dataResponse;
        }

        //helper methods
        private async Task<List<Billing>> CheckIfBillExists(int organisationId, int propertyId, int customerId)
        {

            try
            {
                var custmerbills = await _context.Billing.Where(e => e.OrganisationId.Equals(organisationId) && e.CustomerId.Equals(customerId) && e.PropertyId.Equals(propertyId) && e.Year.Equals(DateTime.Now.Year) && e.BillTypeId == 1).ToListAsync();
                Response dataResponse = new Response();
                if (custmerbills.Count() == 0)
                {
                    List<Billing> bills = new List<Billing>();

                    return bills;
                }

                return custmerbills;
            }
            catch (Exception ex)
            {

                throw;
            }          

           
        }
        public async Task<IEnumerable<GetBillDto>> CreateNonPropertyBillAsync(int organisationId, int customerId, CreateBulkNonProperty createBillDto, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billEntity = _mapper.Map<IEnumerable<Billing>>(createBillDto.CreateNonPropertyBillDto);

            var originalPayerID = await _context.Customers.Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId)).Select(x => x.PayerId).FirstOrDefaultAsync();
            var payerID = originalPayerID.Substring(2);

            var customerAddress = await _context.Customers.Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId))
                                            .Select(x => x.Address).FirstOrDefaultAsync();

            var harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReference(payerID, organisationId);

            foreach (var record in billEntity)
            {
                //get agency and revenue codes
                var agencyCode = await _context.Agencies.Where(x => x.AgencyId.Equals(record.AgencyId) && x.OrganisationId.Equals(organisationId))
                                    .Select(x => x.AgencyCode).FirstOrDefaultAsync();

                var revenueCode = await _context.Revenues.Where(x => x.RevenueId.Equals(record.RevenueId) && x.OrganisationId.Equals(organisationId))
                                    .Select(x => x.RevenueCode).FirstOrDefaultAsync();

                var frequency = await _context.Frequencies.Where(x => x.Id.Equals(record.FrequencyId)).Select(x => x.Frequency).FirstOrDefaultAsync();

                //get current date and future date for applied date
                DateTime currentDate = DateTime.Now;
                string formattedDateTime = currentDate.ToString("yyyy-MM-dd");
                string currentYear = currentDate.Year.ToString();

                //Generate bill reference
                record.BillReferenceNo = BillingUtility.GenerateBillReference(payerID, organisationId);

                //check if record count is greater than 1, if it is generate harmonized bill
                if (billEntity.Count() > 1)
                {
                    int recordCount = billEntity.Count();
                    record.HarmonizedBillReferenceNo = harmonizedBillReference;
                }

                //map bill entity to GenerateBillRequest
                GenerateBillRequest billRequest = new()
                {
                    Amount = record.BillAmount,
                    PayerID = originalPayerID,
                    AgencyRef = agencyCode,
                    RevCode = revenueCode,
                    EntryDate = formattedDateTime,
                    AppliedDate = record.AppliedDate,
                    BillReference = record.BillReferenceNo,
                    HarmonizedBillReference = record.HarmonizedBillReferenceNo,
                    AssessRef = record.BillReferenceNo,
                    Year = currentYear,
                    PropertyAddress = customerAddress
                };

                //call procedure to push bill to EBS-RCM
                BillResponses = _modelService.GenerateBillReference(billRequest);

                record.BillStatusId = 1;
                record.BillTypeId = 2;
                record.FrequencyId = 6;
                record.BillArrears = record.BillAmount;
                record.AmountPaid = 0;
                record.Year = DateTime.Now.Year;
            }

            _repository.Billing.CreateNonPropertyBill(organisationId, customerId, billEntity);
            await _repository.SaveAsync();

            var billToReturn = _mapper.Map<IEnumerable<GetBillDto>>(billEntity);

            return billToReturn;
        }

        public async Task<IEnumerable<GetBillDto>> BackLogBill(int organisationId, int propertyId, int customerId, CreateBulkBacklogBill createBillDto,
            bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billEntity = _mapper.Map<IEnumerable<Billing>>(createBillDto.CreateBacklogBillDto);

            var originalPayerID = await _context.Customers.Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId)).Select(x => x.PayerId).FirstOrDefaultAsync();
            var payerID = originalPayerID.Substring(2);

            var customerAddress = await _context.Customers.Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId))
                                            .Select(x => x.Address).FirstOrDefaultAsync();

            var harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReference(payerID, organisationId);

            foreach (var record in billEntity)
            {
                //get agency and revenue codes
                var agencyCode = await _context.Agencies.Where(x => x.AgencyId.Equals(record.AgencyId) && x.OrganisationId.Equals(organisationId))
                                .Select(x => x.AgencyCode).FirstOrDefaultAsync();

                var revenueCode = await _context.Revenues.Where(x => x.RevenueId.Equals(record.RevenueId) && x.OrganisationId.Equals(organisationId))
                                    .Select(x => x.RevenueCode).FirstOrDefaultAsync();

                //get current date and future date for applied date
                DateTime currentDate = DateTime.Now;
                string formattedDateTime = currentDate.ToString("yyyy-MM-dd");
                string currentYear = currentDate.Year.ToString();

                //Generate bill reference
                record.BillReferenceNo = BillingUtility.GenerateBillReference(payerID, organisationId);

                //check if record count is greater than 1, if it is generate harmonized bill
                if (billEntity.Count() > 1)
                {
                    int recordCount = billEntity.Count();
                    record.HarmonizedBillReferenceNo = harmonizedBillReference;
                }

                //map bill entity to GenerateBillRequest
                GenerateBillRequest billRequest = new()
                {
                    Amount = record.BillAmount,
                    PayerID = originalPayerID,
                    AgencyRef = agencyCode,
                    RevCode = revenueCode,
                    EntryDate = formattedDateTime,
                    AppliedDate = record.AppliedDate,
                    BillReference = record.BillReferenceNo,
                    HarmonizedBillReference = record.HarmonizedBillReferenceNo,
                    AssessRef = record.BillReferenceNo,
                    Year = record.Year.ToString(),
                    PropertyAddress = customerAddress
                };

                //call procedure to push bill to EBS-RCM
                BillResponses = _modelService.GenerateBillReference(billRequest);

                record.BillStatusId = 1;
                record.BillTypeId = 1;
                record.FrequencyId = 6;
                record.BillArrears = record.BillAmount;
                record.AmountPaid = 0;
            }

            _repository.Billing.CreatePropertyBill(organisationId, propertyId, customerId, billEntity);
            await _repository.SaveAsync();

            var billToReturn = _mapper.Map<IEnumerable<GetBillDto>>(billEntity);

            return billToReturn;
        }

        public async Task<IEnumerable<GetBillDto>> AutoBillGeneration(int organisationId, CreateAutoBill createBillDto, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var autoGenerateEntity = _mapper.Map<IEnumerable<Billing>>(createBillDto.CreateAutoBillDto);
            var harmonizedBillReference = "";
            var propertyId = 0;
            var customerId = 0;
            List<Billing> billEntity = new List<Billing>();

            foreach (var record in autoGenerateEntity)
            {
                //get details of bill
                var bills = await _context.Billing.Where(x => x.OrganisationId == organisationId && x.BillId == record.BillId)
                                    .Select(x => x).FirstOrDefaultAsync();

                //fetch customer details
                var customer = await _context.Customers.Where(x => x.OrganisationId == organisationId && x.CustomerId == bills.CustomerId)
                                    .Select(x => x).FirstOrDefaultAsync();

                //get agency and revenue codes
                var agencyCode = await _context.Agencies.Where(x => x.AgencyId.Equals(bills.AgencyId) && x.OrganisationId.Equals(organisationId))
                                    .Select(x => x.AgencyCode).FirstOrDefaultAsync();

                var revenueCode = await _context.Revenues.Where(x => x.RevenueId.Equals(bills.RevenueId) && x.OrganisationId.Equals(organisationId))
                                    .Select(x => x.RevenueCode).FirstOrDefaultAsync();

                var customerAddress = await _context.Customers.Where(x => x.CustomerId.Equals(bills.CustomerId) && x.OrganisationId.Equals(organisationId))
                                           .Select(x => x.Address).FirstOrDefaultAsync();

                propertyId = (int)bills.PropertyId;

                customerId = bills.CustomerId;

                //get year and add plus 1
                var billYear = bills.Year + 1;

                //get current date and future date for applied date
                DateTime currentDate = DateTime.Now;
                string formattedDateTime = currentDate.ToString("yyyy-MM-dd");

                //Generate bill reference
                var BillReferenceNo = BillingUtility.GenerateBillReference(customer.PayerId.Substring(2), organisationId);
                var payerID = customer.PayerId.Substring(2);

                //check if it has harmonized bill reference and generate
                if (!string.IsNullOrEmpty(bills.HarmonizedBillReferenceNo))
                {
                    //generate harmonized bill reference
                    harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReferenceForAutoGeneration(payerID, bills.HarmonizedBillReferenceNo);
                }

                //map bill entity to GenerateBillRequest
                GenerateBillRequest billRequest = new()
                {
                    Amount = bills.BillAmount,
                    PayerID = customer.PayerId,
                    AgencyRef = agencyCode,
                    RevCode = revenueCode,
                    EntryDate = formattedDateTime,
                    AppliedDate = record.AppliedDate,
                    BillReference = BillReferenceNo,
                    HarmonizedBillReference = harmonizedBillReference,
                    AssessRef = BillReferenceNo,
                    Year = billYear.ToString(),
                    PropertyAddress = customerAddress
                };

                //call procedure to push bill to EBS-RCM
                BillResponses = _modelService.GenerateBillReference(billRequest);

                //add record to billEntity
                Billing newBilling = new Billing();
                newBilling.BillStatusId = 1;
                newBilling.BillTypeId = 1;
                newBilling.FrequencyId = 6;
                newBilling.BillArrears = bills.BillAmount;
                newBilling.AmountPaid = 0;
                newBilling.BillAmount = bills.BillAmount;
                newBilling.PropertyId = bills.PropertyId;
                newBilling.CustomerId = bills.CustomerId;
                newBilling.AgencyId = bills.AgencyId;
                newBilling.RevenueId = bills.RevenueId;
                newBilling.Year = billYear;
                newBilling.BillReferenceNo = BillReferenceNo;
                newBilling.HarmonizedBillReferenceNo = harmonizedBillReference;
                newBilling.Category = bills.Category;
                newBilling.DateCreated = DateTime.Now;
                newBilling.CreatedBy = record.CreatedBy;

                billEntity.Add(newBilling);

                _repository.Billing.CreateAutoGeneratedBill(organisationId, propertyId, customerId, newBilling);
                await _repository.SaveAsync();
            }

            var billToReturn = _mapper.Map<IEnumerable<GetBillDto>>(billEntity);

            return billToReturn;
        }

        public async Task<IEnumerable<GetBillDto>> BulkBilling(int organisationId, CreateBulkBillingDto bulkBilling, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var bulkUploadEntity = _mapper.Map<IEnumerable<BulkBillingDto>>(bulkBilling.BulkBillingDto);

            var propertyId = 0;
            var customerId = 0;
            var agencyId = 0;
            var revenueId = 0;
            var harmonizedBillReference = "";
            var harmonizedBillReferenceForPayer = "";
            List<Billing> billEntity = new List<Billing>();

            // Dictionary to store generated harmonized bill references for each PayerID
            var harmonizedBillReferences = new Dictionary<string, string>();

            var payerIdsWithDuplicates = bulkUploadEntity
                .GroupBy(record => record.PayerID.Substring(2))
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            foreach (var payerId in payerIdsWithDuplicates)
            {
                harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReference(payerId, organisationId);
                harmonizedBillReferences[payerId] = harmonizedBillReference;
            }

            foreach (var record in bulkUploadEntity)
            {
                propertyId = _context.Properties.Where(x => x.OrganisationId == organisationId)
                    .FirstOrDefault(p => p.BuildingNo == record.BuildingNo && p.BuildingName == record.BuildingName).PropertyId;

                customerId = _context.Customers.Where(x => x.OrganisationId == organisationId)
                    .FirstOrDefault(p => p.PayerId == record.PayerID).CustomerId;

                agencyId = _context.Agencies.Where(x => x.OrganisationId == organisationId)
                    .FirstOrDefault(p => p.AgencyCode == record.AgencyCode).AgencyId;

                revenueId = _context.Revenues.Where(x => x.OrganisationId == organisationId)
                    .FirstOrDefault(p => p.RevenueCode == record.RevenueCode).RevenueId;

                //get current date and future date for applied date
                DateTime currentDate = DateTime.Now;
                string formattedDateTime = currentDate.ToString("yyyy-MM-dd");
                string currentYear = currentDate.Year.ToString();

                //Generate bill reference
                var BillReferenceNo = BillingUtility.GenerateBillReference(record.PayerID.Substring(2), organisationId);

                if (harmonizedBillReferences.ContainsKey(record.PayerID.Substring(2)))
                {
                    harmonizedBillReferenceForPayer = harmonizedBillReferences[record.PayerID.Substring(2)];
                }

                GenerateBillRequest billRequest = new()
                {
                    Amount = record.BillAmount,
                    PayerID = record.PayerID,
                    AgencyRef = record.AgencyCode,
                    RevCode = record.RevenueCode,
                    EntryDate = formattedDateTime,
                    AppliedDate = record.AppliedDate,
                    BillReference = BillReferenceNo,
                    HarmonizedBillReference = harmonizedBillReferenceForPayer,
                    AssessRef = BillReferenceNo,
                    Year = currentYear,
                    PropertyAddress = $"{record.BuildingNo}, {record.BuildingName}"
                };

                //call procedure to push bill to EBS-RCM
                BillResponses = _modelService.GenerateBillReference(billRequest);

                //add record to billEntity
                Billing newBilling = new Billing();
                newBilling.PropertyId = propertyId;
                newBilling.CustomerId = customerId;
                newBilling.AppliedDate = record.AppliedDate;
                newBilling.BillReferenceNo = BillReferenceNo;
                newBilling.HarmonizedBillReferenceNo = harmonizedBillReferenceForPayer;
                newBilling.BillStatusId = 1;
                newBilling.BillTypeId = 1;
                newBilling.FrequencyId = 6;
                newBilling.BillAmount = record.BillAmount;
                newBilling.BillArrears = record.BillAmount;
                newBilling.AmountPaid = 0;
                newBilling.AgencyId = agencyId;
                newBilling.RevenueId = revenueId;
                newBilling.Category = record.Category;
                newBilling.Year = DateTime.Now.Year;
                newBilling.DateCreated = DateTime.Now;
                newBilling.CreatedBy = record.CreatedBy;

                billEntity.Add(newBilling);
            }

            _repository.Billing.CreatePropertyBill(organisationId, propertyId, customerId, billEntity);
            await _repository.SaveAsync();

            var billToReturn = _mapper.Map<IEnumerable<GetBillDto>>(billEntity);

            return billToReturn;
        }

        public async Task<string> NoOfBillsGeneratedThisMonth(int organisationId)
        {
            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            int countOfGeneratedBills = await _context.Billing.Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfGeneratedBills.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfBillsGeneratedThisWeek(int organisationId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            int countOfGeneratedBills = await _context.Billing.Where(x => x.DateCreated >= startOfWeek && x.DateCreated <= endOfWeek && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfGeneratedBills.ToString("N0");

            return formattedNum;
        }

        public async Task<string> NoOfBillsGeneratedToday(int organisationId)
        {
            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            int countOfGeneratedBills = await _context.Billing.Where(x => x.DateCreated >= startOfDay && x.DateCreated <= endOfDay && x.OrganisationId == organisationId).CountAsync();

            string formattedNum = countOfGeneratedBills.ToString("N0");

            return formattedNum;
        }

        public async Task<DebtReportDto> TotalBilltobePaidToday(int organisationId)
        {

            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day
            var totalAmountOnGeneratedBills = await _context.Billing
                       .Where(x => x.DateCreated >= startOfDay && x.DateCreated <= endOfDay && x.OrganisationId == organisationId).ToListAsync();
            DebtReportDto debtReport = new();
            debtReport.Count = totalAmountOnGeneratedBills.Count(x => x.BillArrears != 0).ToString();
            debtReport.TotalAmount = totalAmountOnGeneratedBills.Sum(x => x.BillArrears).ToString();


            return debtReport;
        }
        public async Task<(IEnumerable<GetBillDto> bills, MetaData metaData)> FilterBillstobePaid(int organisationId, DebtReportParameters param, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            if (!param.ValidDateRange)
                throw new MaxDateRangeBadRequestException();

            var debtMetaData = await _repository.Billing.GetAllBillsAsync(organisationId, param, trackChanges);

            var debtReportdto = _mapper.Map<IEnumerable<GetBillDto>>(debtMetaData);

            return (Billing: debtReportdto, metaData: debtMetaData.MetaData);

        }
        public async Task<DebtReportDto> TotalBilltobePaidThisMonth(int organisationId)
        {

            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            var totalAmountOnGeneratedBills = await _context.Billing
                       .Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.OrganisationId == organisationId).ToListAsync();
            DebtReportDto debtReport = new()
            {
                Count = totalAmountOnGeneratedBills.Count(x => x.BillArrears != 0).ToString(),
                TotalAmount = totalAmountOnGeneratedBills.Sum(x => x.BillArrears).ToString("#,##0.00")
            };


            return debtReport;
        }
        public async Task<DebtReportDto> TotalBilltobePaidThisYear(int organisationId)
        {

            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, 1, 1);
            var endOfMonth = new DateTime(currentMonth.Year, 12, 1).AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            var totalAmountOnGeneratedBills = await _context.Billing
                       .Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.OrganisationId == organisationId).ToListAsync();
            DebtReportDto debtReport = new()
            {
                Count = totalAmountOnGeneratedBills.Count(x => x.BillArrears != 0).ToString(),
                TotalAmount = totalAmountOnGeneratedBills.Sum(x => x.BillArrears).ToString("#,##0.00")
            };


            return debtReport;
        }
        public async Task<string> TotalAmountOfBillsGeneratedThisMonth(int organisationId)
        {
            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            decimal totalAmountOnGeneratedBills = await _context.Billing
                       .Where(x => x.DateCreated >= startOfMonth && x.DateCreated <= endOfMonth && x.OrganisationId == organisationId)
            .SumAsync(x => x.BillAmount);

            string formattedNumber = totalAmountOnGeneratedBills.ToString("#,##0.00");

            return formattedNumber;
        }

        public async Task<string> TotalAmountOfBillsGeneratedThisWeek(int organisationId)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            decimal totalAmountOnGeneratedBills = await _context.Billing
                      .Where(x => x.DateCreated >= startOfWeek && x.DateCreated <= endOfWeek && x.OrganisationId == organisationId)
                      .SumAsync(x => x.BillAmount);

            string formattedNumber = totalAmountOnGeneratedBills.ToString("#,##0.00");

            return formattedNumber;
        }

        public async Task<string> TotalAmountOfBillsGeneratedToday(int organisationId)
        {
            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            decimal totalAmountOnGeneratedBills = await _context.Billing
                      .Where(x => x.DateCreated >= startOfDay && x.DateCreated <= endOfDay && x.OrganisationId == organisationId)
                      .SumAsync(x => x.BillAmount);

            string formattedNumber = totalAmountOnGeneratedBills.ToString("#,##0.00");

            return formattedNumber;
        }

        public async Task<List<AgencyBillingSummaryDto>> NoOfBillsByAreaOffice(int organisationId)
        {
            var summaryList = await _context.Agencies
                .Where(x => x.OrganisationId == organisationId)
                .Select(a => new
                {
                    Id = 0,
                    AgencyName = a.AgencyName,
                    PropertyCount = _context.Billing.Where(p => p.BillTypeId == 1 && p.AgencyId == a.AgencyId).Count(),
                    NonPropertyCount = _context.Billing.Where(p => p.BillTypeId == 2 && p.AgencyId == a.AgencyId).Count(),
                    PropertyAmount = _context.Billing.Where(p => p.BillTypeId == 1 && p.AgencyId == a.AgencyId).Sum(x => x.BillAmount),
                    NonPropertyAmount = _context.Billing.Where(p => p.BillTypeId == 2 && p.AgencyId == a.AgencyId).Sum(x => x.BillAmount),
                }).ToListAsync();

            List<AgencyBillingSummaryDto> result = new List<AgencyBillingSummaryDto>();
            foreach (var item in summaryList)
            {
                result.Add(new AgencyBillingSummaryDto
                {
                    AgencyName = item.AgencyName,
                    PropertyCount = item.PropertyCount.ToString("N0"),
                    PropertyAmount = item.PropertyAmount.ToString("#,##0.00"),
                    NonPropertyCount = item.NonPropertyCount.ToString("N0"),
                    NonPropertyAmount = item.NonPropertyAmount.ToString("#,##0.00")
                });
            }

            // set the Id field to an auto-incrementing value
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id = i + 1;
            }

            return result;
        }

        public async Task<ValidateBillResponseDto> ValidateBill(ValidateBillRequest1Dto validateBill)
        {
            var billReference = _mapper.Map<ValidateBillRequest1Dto>(validateBill);

            var validationUrl = $"{_pidConfig.REV_PAY_URL}/BankPay/Interface/Validate";
            var validationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + validateBill.WebGuid + _pidConfig.STATE);

            ValidateBillRequestDto validateEntity = new ValidateBillRequestDto();
            validateEntity.Hash = validationHash;
            validateEntity.State = _pidConfig.STATE;
            validateEntity.ClientID = _pidConfig.CLIENT_ID;
            validateEntity.TellerID = _pidConfig.TELLER_ID;
            validateEntity.Type = "WEBGuid";
            validateEntity.Currency = "NGN";
            validateEntity.CBNCode = null;
            validateEntity.Date = null;
            validateEntity.WebGuid = billReference.WebGuid;

            //generate PID
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, validationUrl);

            //serialize payload 
            var payload = JsonConvert.SerializeObject(validateEntity);
            var content = new StringContent(payload, null, "application/json");
            request.Content = content;

            //get response
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

            //deserialise response
            ValidateBillResponseDto? output = JsonConvert.DeserializeObject<ValidateBillResponseDto>(responseContent);

            return output;
        }

        public Response ValidateHarmonizedBillReferences(HarmonizedBillReferenceRequestDto harmonizedBill)
        {
            var billReference = _mapper.Map<HarmonizedBillReferenceRequestDto>(harmonizedBill);
            GetHarmonizedBills = _modelService.GetHarmonizedBillReferences(billReference);

            HarmonizedBillReferenceResponseDto harmonizedBillReference = new();
            List<object> harmonizedBillReferenceResult = new List<object>();

            foreach (var response in GetHarmonizedBills)
            {
                //add to list
                harmonizedBillReferenceResult.Add(response);

                harmonizedBillReference.Address = response.Address;
                harmonizedBillReference.EntryID = response.EntryID;
                harmonizedBillReference.PayerType = response.PayerType;
                harmonizedBillReference.PayerID = response.PayerID;
                harmonizedBillReference.AgencyRef = response.AgencyRef;
                harmonizedBillReference.RevCode = response.RevCode;
                harmonizedBillReference.Amount = response.Amount;
                harmonizedBillReference.EntryDate = response.EntryDate;
                harmonizedBillReference.WebGUID = response.WebGUID;
                harmonizedBillReference.AssessRef = response.AssessRef;
                harmonizedBillReference.Notes = response.Notes;
                harmonizedBillReference.Sysdate = response.Sysdate;
                harmonizedBillReference.fullname = response.fullname;
                harmonizedBillReference.Address = response.Address;
                harmonizedBillReference.email = response.email;
                harmonizedBillReference.gsm = response.gsm;
                harmonizedBillReference.BillOwner = response.BillOwner;
                harmonizedBillReference.PmtStus = response.PmtStus;
                harmonizedBillReference.BulkReference = response.BulkReference;
                harmonizedBillReference.BulkTotalAmount = response.BulkTotalAmount;
                harmonizedBillReference.Pmt_Flag = response.Pmt_Flag;
            }

            Response dataResponse = new();

            if (harmonizedBillReference.BulkReference != null)
            {
                dataResponse.StatusMessage = "Harmonized Bill Reference Record Found";
                dataResponse.Data = harmonizedBillReferenceResult;
                dataResponse.Status = 200;
            }

            else
            {
                dataResponse.StatusMessage = "Harmonized Bill Reference Record Not Found";
                dataResponse.Status = 404;
                dataResponse.Data = null;
            }

            return dataResponse;
        }

        public async Task<List<BillingReportDto>> GenerateBillReport(int organisationId, long billId)
        {
            List<BillingReportDto> result = new List<BillingReportDto>();

            var bill = await _context.Billing.FindAsync(billId);

            if (bill != null)
            {
                if (string.IsNullOrEmpty(bill.HarmonizedBillReferenceNo))
                {
                    //get bill details
                    var pdfBill = await _context.Billing.Where(x => x.BillId.Equals(billId) && x.OrganisationId.Equals(organisationId))
                                     .Select(a => new
                                     {
                                         Property = _context.Properties.Where(c => c.PropertyId == a.PropertyId).FirstOrDefault(),
                                         Customer = _context.Customers.Where(c => c.CustomerId == a.CustomerId).FirstOrDefault(),
                                         Revenue = _context.Revenues.Where(p => p.RevenueId == a.RevenueId).FirstOrDefault(),
                                         Agency = _context.Agencies.Where(p => p.AgencyId == a.Property.AgencyId).FirstOrDefault(),
                                         Organisation = _context.Organisations.Where(p => p.OrganisationId == organisationId).FirstOrDefault(),
                                         BillFormat = _context.BillFormats.Where(p => p.OrganisationId == organisationId).FirstOrDefault(),
                                         Category= _context.Category.Where (c => c.CategoryName==a.Category).FirstOrDefault()
                                     }).ToListAsync();

                    //Generate QR Code Image 
                    var content = $"Bill Reference: {bill.BillReferenceNo}\nAmount: ₦{bill.BillAmount}\nYear: {bill.Year}";

                    var qrCodeData = $"https://kuje.credodemo.com/?ref={bill.BillReferenceNo}&type=webguid";
                    using var generator = new QRCodeGenerator();
                    var level = ECCLevel.H;
                    var qr = generator.CreateQrCode(qrCodeData, level);

                    // Render to canvas
                    var info = new SKImageInfo(512, 512);
                    using var surface = SKSurface.Create(info);
                    var canvas = surface.Canvas;
                    canvas.Render(qr, info.Width, info.Height);

                    using var image = surface.Snapshot();
                    using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                    // Convert the image data to a byte array
                    byte[] imageBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        data.SaveTo(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }

                    foreach (var item in pdfBill)
                    {
                        result.Add(new BillingReportDto
                        {
                            PropertyName = item.Property?.BuildingName,
                            PropertyAddress = item.Property?.LocationAddress,
                            LastName = item.Customer?.LastName,
                            FirstName = item.Customer?.FirstName,
                            MiddleName = item.Customer?.MiddleName,
                            CustomerAddress = item.Customer?.Address,
                            PayerID = item.Customer?.PayerId,
                            AreaOffice = item.Agency?.AgencyName,
                            GeneratedDate = item.Property.DateCreated,
                            OrganisationName = item.Organisation?.OrganisationName,
                            OrganisationLogo = item.Organisation?.LogoData,
                            Summary = item.Revenue.RevenueName,
                            Year = bill.Year.ToString(),
                            BillReference = bill.BillReferenceNo,
                            Debit = bill.BillAmount,
                            Credit = 0,
                            Arrears = (decimal)bill.BillArrears,
                            AmountPaid = bill.AmountPaid,
                            Balance = bill.BillAmount,
                            OrganisationAddress = item.Organisation.Address,
                            OrganisationPhoneNumber = item.Organisation.PhoneNo,
                            OrganisationEmail = item.Organisation.Email,
                            BarCode = imageBytes,
                            SignatureOne = item.BillFormat.SignatureOneData,
                            SignatureTwo = item.BillFormat.SignatureTwoData,
                            CategoryName =item.Category==null? "" : item.Category.CategoryName 
                        });
                    }
                }

                else
                {
                    // Multiple bills with the same harmonizedBillReference
                    var billsWithSameReference = _context.Billing
                        .Where(b => b.HarmonizedBillReferenceNo == bill.HarmonizedBillReferenceNo && b.OrganisationId.Equals(organisationId))
                        .ToList();

                    foreach (var bills in billsWithSameReference)
                    {
                        var pdfBill = new
                        {
                            Property = _context.Properties.FirstOrDefault(c => c.PropertyId == bills.PropertyId),
                            Customer = _context.Customers.FirstOrDefault(c => c.CustomerId == bills.CustomerId),
                            Revenue = _context.Revenues.FirstOrDefault(p => p.RevenueId == bills.RevenueId),
                            Agency = _context.Agencies.FirstOrDefault(p => p.AgencyId == bills.Property.AgencyId),
                            Organisation = _context.Organisations.FirstOrDefault(p => p.OrganisationId == organisationId),
                            BillFormat = _context.BillFormats.FirstOrDefault(p => p.OrganisationId == organisationId),
                            Category = _context.Category.Where(c => c.CategoryName == bills.Category).FirstOrDefault()
                        };

                        var totalAmount = +bills.BillAmount;

                        // Generate QR Code Image 
                        var content = $"Bill Reference: {bill.HarmonizedBillReferenceNo}\nAmount: ₦{totalAmount}\nYear: {bill.Year}";
                        var qrCodeData = $"https://kuje.credodemo.com/?ref={bill.HarmonizedBillReferenceNo}&type=harmonized";

                        using var generator = new QRCodeGenerator();
                        var level = ECCLevel.H;
                        var qr = generator.CreateQrCode(qrCodeData, level);

                        // Render to canvas
                        var info = new SKImageInfo(512, 512);
                        using var surface = SKSurface.Create(info);
                        var canvas = surface.Canvas;
                        canvas.Render(qr, info.Width, info.Height);

                        using var image = surface.Snapshot();
                        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                        // Convert the image data to a byte array
                        byte[] imageBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            data.SaveTo(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }

                        result.Add(new BillingReportDto
                        {
                            PropertyName = pdfBill.Property?.BuildingName,
                            PropertyAddress = pdfBill.Property?.LocationAddress,
                            LastName = pdfBill.Customer?.LastName,
                            FirstName = pdfBill.Customer?.FirstName,
                            MiddleName = pdfBill.Customer?.MiddleName,
                            CustomerAddress = pdfBill.Customer?.Address,
                            PayerID = pdfBill.Customer?.PayerId,
                            AreaOffice = pdfBill.Agency?.AgencyName,
                            GeneratedDate = pdfBill.Property.DateCreated,
                            OrganisationName = pdfBill.Organisation?.OrganisationName,
                            OrganisationLogo = pdfBill.Organisation?.LogoData,
                            Summary = pdfBill.Revenue.RevenueName,
                            Year = bills.Year.ToString(),
                            BillReference = bills.BillReferenceNo,
                            HarmonizedBillReference = bills.HarmonizedBillReferenceNo,
                            Debit = bills.BillAmount,
                            Credit = 0,
                            //Arrears
                            Balance = bills.BillAmount,
                            OrganisationAddress = pdfBill.Organisation.Address,
                            OrganisationPhoneNumber = pdfBill.Organisation.PhoneNo,
                            OrganisationEmail = pdfBill.Organisation.Email,
                            BarCode = imageBytes,
                            SignatureOne = pdfBill.BillFormat.SignatureOneData,
                            SignatureTwo = pdfBill.BillFormat.SignatureTwoData,
                            ContentOne = pdfBill.BillFormat.ContentOne,
                            ContentTwo = pdfBill.BillFormat.ContentTwo,
                            ContentThree = pdfBill.BillFormat.ClosingSection,
                            CategoryName = pdfBill.Category == null ? "" : pdfBill.Category.CategoryName
                        });
                    }
                }
            }

            return result;
        }
        public async Task<List<BillingReportDto>> GenerateBillReport(int organisationId, string harmonized)
        {
            List<BillingReportDto> result = new List<BillingReportDto>();

            // Multiple bills with the same harmonizedBillReference
            var billsWithSameReference = _context.Billing
                .Where(b => b.HarmonizedBillReferenceNo == harmonized && b.OrganisationId.Equals(organisationId) && b.Year.Equals(DateTime.Now.Year))
                .ToList();
            if (billsWithSameReference.Count > 0)
            {



                foreach (var bills in billsWithSameReference)
                {
                    var pdfBill = new
                    {
                        Property = _context.Properties.FirstOrDefault(c => c.PropertyId == bills.PropertyId),
                        Customer = _context.Customers.FirstOrDefault(c => c.CustomerId == bills.CustomerId),
                        Revenue = _context.Revenues.FirstOrDefault(p => p.RevenueId == bills.RevenueId),
                        Agency = _context.Agencies.FirstOrDefault(p => p.AgencyId == bills.Property.AgencyId),
                        Organisation = _context.Organisations.FirstOrDefault(p => p.OrganisationId == organisationId),
                        BillFormat = _context.BillFormats.FirstOrDefault(p => p.OrganisationId == organisationId),
                    };

                    var totalAmount = +bills.BillAmount;

                    // Generate QR Code Image 
                    var content = $"Bill Reference: {bills.BillId}\nAmount: ₦{totalAmount}\nYear: {bills.Year}";

                    var qrCodeData = $"https://kuje.credodemo.com/?ref={bills.BillId}&type=harmonized";

                    using var generator = new QRCodeGenerator();
                    var level = ECCLevel.H;
                    var qr = generator.CreateQrCode(qrCodeData, level);

                    // Render to canvas
                    var info = new SKImageInfo(512, 512);
                    using var surface = SKSurface.Create(info);
                    var canvas = surface.Canvas;
                    canvas.Render(qr, info.Width, info.Height);

                    using var image = surface.Snapshot();
                    using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                    // Convert the image data to a byte array
                    byte[] imageBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        data.SaveTo(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }

                    result.Add(new BillingReportDto
                    {
                        PropertyName = pdfBill.Property?.BuildingName,
                        PropertyAddress = pdfBill.Property?.LocationAddress,
                        LastName = pdfBill.Customer?.LastName,
                        FirstName = pdfBill.Customer?.FirstName,
                        MiddleName = pdfBill.Customer?.MiddleName,
                        CustomerAddress = pdfBill.Customer?.Address,
                        PayerID = pdfBill.Customer?.PayerId,
                        AreaOffice = pdfBill.Agency?.AgencyName,
                        GeneratedDate = pdfBill.Property.DateCreated,
                        OrganisationName = pdfBill.Organisation?.OrganisationName,
                        OrganisationLogo = pdfBill.Organisation?.LogoData,
                        Summary = pdfBill.Revenue.RevenueName,
                        Year = bills.Year.ToString(),
                        BillReference = bills.BillReferenceNo,
                        HarmonizedBillReference = bills.HarmonizedBillReferenceNo,
                        Debit = bills.BillAmount,
                        Credit = 0,
                        //Arrears
                        Balance = bills.BillAmount,
                        OrganisationAddress = pdfBill.Organisation.Address,
                        OrganisationPhoneNumber = pdfBill.Organisation.PhoneNo,
                        OrganisationEmail = pdfBill.Organisation.Email,
                        BarCode = imageBytes,
                        SignatureOne = pdfBill.BillFormat.SignatureOneData,
                        SignatureTwo = pdfBill.BillFormat.SignatureTwoData,
                        ContentOne = pdfBill.BillFormat.ContentOne,
                        ContentTwo = pdfBill.BillFormat.ContentTwo,
                        ContentThree = pdfBill.BillFormat.ClosingSection,
                    });
                }
            }


            return result;
        }


        public async Task<Response> CreateBillFormatAsync(int organisationId, CreateBillFormat createBillFormat, bool trackChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billEntity = _mapper.Map<BillFormat>(createBillFormat);

            //create file attachment for signature ogit ne
            if (createBillFormat.SignatureOne != null)
            {
                if (createBillFormat.SignatureOne.Length > 0)
                {
                    LogoFileName = Path.GetFileName(createBillFormat.SignatureOne.FileName);
                    LogoFileExtension = Path.GetExtension(LogoFileName);
                    LogoNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension);
                }

                if (createBillFormat.SignatureOne.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 200KB";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                if (!_extensions.Contains(LogoFileExtension.ToLower()))
                {
                    dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                //convert to file to memory stream 
                using (var ms = new MemoryStream())
                {
                    createBillFormat.SignatureOne.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    billEntity.SignatureOneData = fileBytes;
                    billEntity.SignatureOneName = createBillFormat.SignatureOne.FileName;
                }
            }

            //create file attachment for signature two
            if (createBillFormat.SignatureTwo != null)
            {
                if (createBillFormat.SignatureTwo.Length > 0)
                {
                    LogoFileName1 = Path.GetFileName(createBillFormat.SignatureTwo.FileName);
                    LogoFileExtension1 = Path.GetExtension(LogoFileName1);
                    LogoNewFileName1 = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension1);
                }

                if (createBillFormat.SignatureTwo.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 200KB";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                if (!_extensions.Contains(LogoFileExtension1.ToLower()))
                {
                    dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                //convert to file to memory stream 
                using (var ms = new MemoryStream())
                {
                    createBillFormat.SignatureTwo.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    billEntity.SignatureTwoData = fileBytes;
                    billEntity.SignatureTwoName = createBillFormat.SignatureTwo.FileName;
                }
            }

            _repository.BillFormat.CreateBillFormat(organisationId, billEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Bill Format Created Successfully";
            dataResponse.Status = 200;
            dataResponse.Data = billEntity;

            return dataResponse;
        }

        public async Task<Response> UpdateBillFormatAsync(int organisationId, int billFormatId, UpdateBillFormat updateBillFormat, bool trackChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billEntity = await _repository.BillFormat.GetBillFormatAsync(organisationId, billFormatId, trackChanges);
            if (billEntity is null)
                throw new IdNotFoundException("Bill Format", billFormatId);

            //create file attachment for signature one
            if (updateBillFormat.SignatureOne != null)
            {
                if (updateBillFormat.SignatureOne.Length > 0)
                {
                    LogoFileName = Path.GetFileName(updateBillFormat.SignatureOne.FileName);
                    LogoFileExtension = Path.GetExtension(LogoFileName);
                    LogoNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension);
                }

                if (updateBillFormat.SignatureOne.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 200KB";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                if (!_extensions.Contains(LogoFileExtension.ToLower()))
                {
                    dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                //convert to file to memory stream 
                using (var ms = new MemoryStream())
                {
                    updateBillFormat.SignatureOne.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    billEntity.SignatureOneData = fileBytes;
                    billEntity.SignatureOneName = updateBillFormat.SignatureOne.FileName;
                }
            }

            //create file attachment for signature two
            if (updateBillFormat.SignatureTwo != null)
            {
                if (updateBillFormat.SignatureTwo.Length > 0)
                {
                    LogoFileName1 = Path.GetFileName(updateBillFormat.SignatureTwo.FileName);
                    LogoFileExtension1 = Path.GetExtension(LogoFileName1);
                    LogoNewFileName1 = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension1);
                }

                if (updateBillFormat.SignatureTwo.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 200KB";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                if (!_extensions.Contains(LogoFileExtension1.ToLower()))
                {
                    dataResponse.StatusMessage = "Only image files are allowed (.jpg, .jpeg, .png)";
                    dataResponse.Status = 400;

                    return dataResponse;
                }

                //convert to file to memory stream 
                using (var ms = new MemoryStream())
                {
                    updateBillFormat.SignatureTwo.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    billEntity.SignatureTwoData = fileBytes;
                    billEntity.SignatureTwoName = updateBillFormat.SignatureTwo.FileName;
                }
            }
            if (updateBillFormat.SignatureOne == null || updateBillFormat.SignatureTwo == null)
            {
                billEntity.ContentOne = updateBillFormat.ContentOne;
                billEntity.ContentTwo = updateBillFormat.ContentTwo;
                billEntity.ClosingSection = updateBillFormat.ClosingSection;
                billEntity.DateModified = updateBillFormat.DateModified;
                billEntity.ModifiedBy = updateBillFormat.ModifiedBy;
            }
            else
            {
                _mapper.Map(updateBillFormat, billEntity);
            }
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Bill Format Updated Successfully";
            dataResponse.Status = 200;
            dataResponse.Data = billEntity;

            return dataResponse;
        }

        public async Task<(IEnumerable<GetBillFormat> bills, MetaData metaData)> GetAllBillFormatsAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billsWithMetaData = await _repository.BillFormat.GetAllBillFormatsAsync(organisationId, requestParameters, trackChanges);

            var billsDto = _mapper.Map<IEnumerable<GetBillFormat>>(billsWithMetaData);

            return (bills: billsDto, metaData: billsWithMetaData.MetaData);
        }

        public async Task<GetBillFormat> GetBillFormatAsync(int organisationId, int billFormatId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var billFormat = await _repository.BillFormat.GetBillFormatAsync(organisationId, billFormatId, trackChanges);
            if (billFormat is null)
                throw new IdNotFoundException("Bill Format", billFormatId);

            var billFormatDto = _mapper.Map<GetBillFormat>(billFormat);

            return billFormatDto;
        }

        public Task<Response> StepDownBill(int organisationId, int billId, StepDownBillDto stepDown)
        {
            throw new NotImplementedException();
        }

        public Task<string> CountOfSteppedDownBills(int organisationId, int billId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UploadBillsAsync(int organisationId, string creator, DefaultParameters requestParameters, IFormFile file)
        {
            List<CreatePropertyBillUpload> createProperty = new List<CreatePropertyBillUpload>();
            List<CreatePropertyBillUpload> existproperty = new List<CreatePropertyBillUpload>();
            Response response = new Response();
            var excelImporter = new ExcelHelper();

            var filePath = SaveFile(file);

            var billRequests = excelImporter.ImportExcel<CreatePropertyBillUpload>(filePath);
            if (billRequests.Count > 2)
            {
                response.Status = 0;
                response.StatusMessage = "Bills uploaded cannot be greater than 50";
                return (response);
            }
            else
            {
                foreach (var item in billRequests)
                {
                    createProperty.Add(item);
                }
                response.Status = 200;
                response.StatusMessage = "Bills uploaded successfully";
                response.Data = createProperty;
                return (response);
            }
            return (response);
        }
        public async Task<List<PreviewedbillResponse>> BulkPreviewedBilling(int organisationId, string createdby, IEnumerable<CreatePropertyBillUpload> previewedbills, bool trackChanges)
        {
            List<PreviewedbillResponse> billerrors = new List<PreviewedbillResponse>();
            Property property = new Property();
            int customer = 0;
            int businessType = 0;
            int revenue = 0;
            int agencyId = 0;
            string payerId = "";
            int counter = 0;

            // Check that the agency exists
            agencyId = await CheckAgency(previewedbills.FirstOrDefault().Agency);
            if (agencyId == 0)
            {
                billerrors.Add(new PreviewedbillResponse
                {
                    Bill = null,
                    StatusMessage = "Agency does not exist!"
                });
            }
            else
            {
                foreach (var item in previewedbills)
                {
                    counter++;

                    // Check that business type exists
                    businessType = await CheckBusinessType(organisationId, item.BusinessType);
                    if (businessType == 0)
                    {
                        billerrors.Add(new PreviewedbillResponse
                        {
                            itemId = counter,
                            Bill = item,
                            StatusMessage = "Business Type does not exist"
                        });
                        continue;
                    }

                    // Check that revenue exists
                    revenue = await CheckRevenue(organisationId, item.RevenueCode);
                    if (revenue == 0)
                    {
                        billerrors.Add(new PreviewedbillResponse
                        {
                            itemId = counter,
                            Bill = item,
                            StatusMessage = "Revenue does not exist"
                        });
                        continue;
                    }

                    // Check that property exists
                    var propertyResp = await CheckIfPropertyExists(organisationId, agencyId, item);
                    property = (Property)propertyResp.Data;

                    if (property.PropertyId == 0)
                    {
                        // Check if street exists
                        var street = await CheckStreet(item.StreetName, agencyId);
                        if (street == 0)
                        {
                            billerrors.Add(new PreviewedbillResponse
                            {
                                itemId = counter,
                                Bill = item,
                                StatusMessage = "Street does not exist!"
                            });
                            continue;
                        }

                        // Create property if it doesn't exist
                        if (street != 0)
                        {
                            property = await CreateProperty(organisationId, createdby, street, agencyId, property.SpaceIdentifierId, item);
                        }
                    }

                    if (property.PropertyId != 0)
                    {
                        if (string.IsNullOrEmpty(item.PayerID) || item.PayerID == "null")
                        {
                            // Check if PayerID exists or create one
                            var payer = await CheckPayer(item);

                            // Log what CheckPayer returns for debugging
                            Console.WriteLine($"CheckPayer Result: Data = {payer?.Data}, StatusMessage = {payer?.StatusMessage}");

                            // Ensure that payerId is being assigned correctly
                            if (payer?.Data != null)
                            {
                                payerId = payer.Data.ToString();
                            }
                            else
                            {
                                payerId = null;
                            }

                            if (payerId == null)
                            {
                                billerrors.Add(new PreviewedbillResponse
                                {
                                    itemId = counter,
                                    Bill = item,
                                    StatusMessage = payer?.StatusMessage ?? "Failed to create or retrieve PayerID"
                                });
                                continue;
                            }
                        }
                        else
                        {
                            payerId = item.PayerID;
                        }

                        // Check that customer exists and create the bill
                        customer = await CheckCustomer(organisationId, property.PropertyId, createdby, payerId, item);
                        var billbyname = await CheckBillExistsandCreate(organisationId, property.PropertyId, agencyId, customer, payerId, item, createdby, businessType, revenue);

                        if (billbyname.Status == 409)
                        {
                            billerrors.Add(new PreviewedbillResponse
                            {
                                itemId = counter,
                                Bill = item,
                                StatusMessage = "Bill already exists"
                            });
                        }
                        else
                        {
                            billerrors.Add(new PreviewedbillResponse
                            {
                                itemId = counter,
                                Bill = item,
                                StatusMessage = billbyname.StatusMessage,
                                Data = billbyname.Data,
                                Status = billbyname.Status
                            });
                        }
                    }
                }
            }

            return billerrors;
        }

        //Get bill GetSpaceIdentifierId
        private async Task<Response> CheckBillExistsandCreate(int organisationId, int property, int agencyId, int customer, string payerId, CreatePropertyBillUpload createPropertyBill, string createdby, int businesstypeId, int revenueId)
        {
            Response response = new Response();
            string harmonizedBillReference = null;
            var bills = await _context.Billing.Where(x => x.CustomerId.Equals(customer) && x.OrganisationId.Equals(organisationId) && x.PropertyId.Equals(property)
                       && x.AppliedDate.Equals(DateTime.Now.Year.ToString())).ToListAsync();
            var resp = bills.Where(a => a.BusinessTypeId.Equals(businesstypeId) && a.RevenueId.Equals(revenueId)).ToList();
            if (bills.Count > 1)
            {
                harmonizedBillReference = bills.FirstOrDefault().HarmonizedBillReferenceNo;
            }
            if (bills.Count > 0 && resp.Count == 1)
            {
                response.Status = 409;
            }
            else if (bills.Count >= 0 && resp.Count == 0)
            {
                if (bills.Count == 1)
                {
                    //if a bill exists that is not same with the new item the
                    harmonizedBillReference = BillingUtility.GenerateHarmonizedBillReference(payerId, organisationId);
                    var billItem = bills.SingleOrDefault();
                    billItem.HarmonizedBillReferenceNo = harmonizedBillReference;
                    _context.Billing.Update(billItem);
                    _context.SaveChanges();
                    response.Data = harmonizedBillReference;


                }
                else if (bills.Count > 1)
                {
                    response.Data = bills.FirstOrDefault().HarmonizedBillReferenceNo;


                }
                Billing billEntity = new Billing();
                //get current date and future date for applied date
                DateTime currentDate = DateTime.Now;
                string formattedDateTime = currentDate.ToString("yyyy-MM-dd");
                string currentYear = currentDate.Year.ToString();
                var payerNo = payerId.Substring(2);
                //Generate bill reference
                var billReferenceNo = BillingUtility.GenerateBillReference(payerNo, organisationId);
                var businesssizeId = await GetBusinessTypeId(organisationId, createPropertyBill.BusinessSize);
                var billAmount = await GetRevenuePrice(organisationId, createPropertyBill.Category, businesssizeId, createPropertyBill.PayerType);

                if (billAmount != 0)
                {
                    GenerateBillRequest billRequest = new()
                    {
                        Amount = billAmount,
                        PayerID = payerId,
                        AgencyRef = createPropertyBill.Agency,
                        RevCode = createPropertyBill.Revenue,
                        EntryDate = formattedDateTime,
                        AppliedDate = createPropertyBill.AppliedDate,
                        BillReference = billReferenceNo,
                        HarmonizedBillReference = harmonizedBillReference,
                        AssessRef = billReferenceNo,
                        Year = currentYear,
                        PropertyAddress = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName
                    };

                    //call procedure to push bill to EBS-RCM
                    BillResponses = _modelService.GenerateBillReference(billRequest);
                    billEntity.BusinessTypeId = businesstypeId;
                    billEntity.AgencyId = agencyId;
                    billEntity.AppliedDate = createPropertyBill.AppliedDate;
                    billEntity.DateCreated = DateTime.Now;
                    billEntity.RevenueId = revenueId;
                    billEntity.Category = createPropertyBill.Category;
                    billEntity.BillAmount = billAmount;
                    billEntity.BusinessSizeId = businesssizeId;
                    billEntity.BillStatusId = 1;
                    billEntity.BillTypeId = 1;
                    billEntity.FrequencyId = 6;
                    billEntity.BillReferenceNo = BillResponses.FirstOrDefault().bankreference;
                    billEntity.BillArrears = billAmount;
                    billEntity.AmountPaid = 0;
                    billEntity.Billbf = 0;
                    billEntity.Year = DateTime.Now.Year;
                    billEntity.CreatedBy = createdby;
                    billEntity.CustomerId = customer;
                    billEntity.OrganisationId = organisationId;
                    billEntity.PropertyId = property;
                    billEntity.RevenueId = revenueId;
                    billEntity.ModifiedBy = createdby;
                    _repository.Billing.CreateBill(organisationId, property, customer, billEntity);
                    await _repository.SaveAsync();

                    if (bills.Count == 0)
                    {

                        //send mail to custmer

                        MailRequest mailRequest = new MailRequest();
                        mailRequest.Subject = "Billing";
                        mailRequest.ToEmail = createPropertyBill.Email;
                        mailRequest.Body = "";//billEntity;
                        mailRequest.FirstName = createPropertyBill.FirstName;
                        mailRequest.LastName = createPropertyBill.LastName;
                        await _mailService.SendBillGenerationAsync(mailRequest);
                        var billId = billEntity.BillId;
                        response.StatusMessage = "Bill generated successfully";
                        response.Data = billId;
                        response.Status = 200;
                    }
                    else
                    {
                        //send mail to custmer

                        MailRequest mailRequest = new MailRequest();
                        mailRequest.Subject = "Billing";
                        mailRequest.ToEmail = createPropertyBill.Email;
                        mailRequest.Body = "";//billEntity;
                        mailRequest.FirstName = createPropertyBill.FirstName;
                        mailRequest.LastName = createPropertyBill.LastName;
                        await _mailService.SendBillGenerationAsync(mailRequest);
                        response.StatusMessage = "Bill generated successfully";
                        response.Data = harmonizedBillReference;
                        response.Status = 200;
                    }

                }
                else
                {
                    response.Status = 0;
                    response.StatusMessage = "Bill category does not return an amount";

                }

            }



            return response;
        }
        //check PayerId
        private async Task<Response> CheckPayer(CreatePropertyBillUpload createPropertyBill)
        {
            Response payer = new Response();
            GetTaxPayerRequestDto pidEntity = new GetTaxPayerRequestDto();
            GetTaxPayerRequestDto getTaxPayerRequestDto = new GetTaxPayerRequestDto();

            // Check by Email
            if (payer.Data == null && createPropertyBill.Email != null)
            {
                getTaxPayerRequestDto.Param = createPropertyBill.Email;
                pidEntity = _mapper.Map<GetTaxPayerRequestDto>(getTaxPayerRequestDto);
                var result = _modelService.GetCustomerDetailsByEmail(pidEntity);
                payer.Data = result.FirstOrDefault()?.PayerID;
            }

            // Check by Phone Number
            if (payer.Data == null && createPropertyBill.PhoneNumber != null)
            {
                getTaxPayerRequestDto.Param = createPropertyBill.PhoneNumber;
                pidEntity = _mapper.Map<GetTaxPayerRequestDto>(getTaxPayerRequestDto);
                var result1 = _modelService.GetCustomerDetailsByPhoneNumber(pidEntity);
                payer.Data = result1.FirstOrDefault()?.PayerID;
            }

            // Check by Full Name
            if (payer.Data == null && createPropertyBill.FullName != null)
            {
                getTaxPayerRequestDto.Param = createPropertyBill.FullName;
                pidEntity = _mapper.Map<GetTaxPayerRequestDto>(getTaxPayerRequestDto);
                var result2 = _modelService.GetCustomerDetailsByPhoneNumber(pidEntity); // Fixed method name
                payer.Data = result2.FirstOrDefault()?.PayerID;
            }

            // If no Payer ID found, attempt to create one
            if (payer.Data == null)
            {
                if (createPropertyBill.PayerType == "C")
                {
                    // Corporate payer creation
                    CorporatePayerIDRequest corporatePayerID = new CorporatePayerIDRequest
                    {
                        CompanyName = createPropertyBill.FullName,
                        PhoneNumber = createPropertyBill.PhoneNumber,
                        Email = createPropertyBill.Email,
                        Address = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName,
                        DateofIncorporation = DateTime.Now.ToString("MM/dd/yyyy")
                    };
                    var corpEntity = _mapper.Map<CorporatePayerIDRequest>(corporatePayerID);

                    // Create the corporate PID and get the results
                    var result3 = _modelService.CreateCorporatePID(corpEntity);
                    var firstResult = result3.FirstOrDefault();

                    // Check if the first result is not null and process the outData
                    if (firstResult != null)
                    {
                        var payerIdExtractor = new PayerIdExtractor();

                        var test1 = payerIdExtractor.GetPayerId(firstResult.outData);
                        if (test1.Successful)
                        {

                            payer.Data = test1.PayerId; // Set the payer ID
                        }

                        //if (firstResult.outData.Contains("C-") || firstResult.outData.Contains("N-"))
                        //{
                        //    payer.Data = firstResult.outData; // Set the payer ID
                        //}

                        // Always set payer.StatusMessage regardless of the condition
                        payer.StatusMessage = firstResult.outData;
                    }
                }
                else
                {
                    // Individual payer creation
                    CustomerEnumerationDto enumerationDto = new CustomerEnumerationDto
                    {
                        Type = createPropertyBill.PayerType,
                        Hash = "",
                        Title = "Mr",
                        Sex = "M",
                        LastName = createPropertyBill.LastName,
                        FirstName = createPropertyBill.FirstName,
                        OtherName = createPropertyBill.MiddleName,
                        MaritalStatus = "M",
                        DateOfBirth = DateTime.Now.ToString(),
                        Phone = createPropertyBill.PhoneNumber,
                        Email = createPropertyBill.Email,
                        Address = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName,
                        State = ""
                    };
                    var result2 = await _modelService.CreatePIDWithBioData(enumerationDto);

                    if (result2.Pid.StartsWith("C") || result2.Pid.StartsWith("N"))
                    {
                        payer.Data = result2.Pid; // Set the payer ID
                    }
                    payer.StatusMessage = result2.StatusMessage; // Set the status message
                }
            }

            return payer;
        }


        //Create Customer
        private async Task<int> CreateCustomer(int organisationId, string createdby, string payerId, CreatePropertyBillUpload createBill)
        {
            CreateCustomerDto createcustumer = new CreateCustomerDto();
            createcustumer.Address = createBill.BuildingNumber + " " + createBill.StreetName;

            if (createBill.PayerType == "N")
            {
                createcustumer.PayerTypeId = 1;
            }
            else
            {
                createcustumer.PayerTypeId = 2;
                createcustumer.CorporateName = createBill.FullName;
            }
            createcustumer.PayerId = payerId;
            createcustumer.TitleId = 1;
            createcustumer.FirstName = createBill.FirstName;
            createcustumer.LastName = createBill.LastName;
            createcustumer.MiddleName = createBill.MiddleName;
            createcustumer.GenderId = 1;
            createcustumer.MaritalStatusId = 2;
            createcustumer.Address = createBill.BuildingNumber + " " + createBill.StreetName;
            createcustumer.Email = createBill.Email;
            createcustumer.PhoneNo = createBill.PhoneNumber;
            createcustumer.SuppliedPID = true;
            createcustumer.DateCreated = DateTime.Now;
            createcustumer.CreatedBy = createdby;
            var customerEntity = _mapper.Map<Customers>(createcustumer);
            customerEntity.FullName = createBill.FullName;

            _repository.Customer.CreateCustomer(organisationId, customerEntity);
            await _repository.SaveAsync();

            return customerEntity.CustomerId;

        }
        //create property
        private async Task<Property> CreateProperty(int organisationId, string createdby, int streetId, int agency, int spaceIdentifierId, CreatePropertyBillUpload createPropertyBill)
        {

            CreatePropertyDto createProperty = new CreatePropertyDto();
            createProperty.AgencyId = agency;
            createProperty.SpaceIdentifierId = spaceIdentifierId;
            createProperty.StreetId = streetId;
            createProperty.LocationAddress = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName;
            createProperty.SpaceFloor = Int32.Parse(createPropertyBill.SpaceFloor);
            createProperty.BuildingNo = createPropertyBill.BuildingNumber;
            createProperty.BuildingName = createPropertyBill.BuildingName;
            createProperty.DateCreated = DateTime.Now;
            createProperty.CreatedBy = createdby;
            var propertyEntity = _mapper.Map<Entities.Models.Property>(createProperty);
            _repository.Property.CreateProperty(organisationId, propertyEntity);
            await _repository.SaveAsync();


            return propertyEntity;
        }
        ////GetWard
        //private async Task<int> GetWard(string ward)
        //{
        //    var identifier = await _context.Wards.Where(x => x.WardName.Equals(ward)).SingleOrDefaultAsync();
        //    if (identifier == null)
        //    {
        //        return 0;
        //    }
        //    return identifier.Id;
        //}

        //check street exist
        private async Task<int> CheckStreet(string streetName, int agency)
        {
            var street = await _context.Streets.Where(x => x.StreetName.Equals(streetName) && x.AgencyId.Equals(agency)).FirstOrDefaultAsync();

            if (street == null)
            {
                return 0;
            }
            return street.StreetId;
        }
        //Get spaceIdentifier Id
        private async Task<int> GetSpaceIdentifierId(int organisationId, string spaceIdentifier)
        {
            var identifier = await _context.SpaceIdentifiers.Where(x => x.SpaceIdentifierName.Equals(spaceIdentifier) && organisationId.Equals(organisationId)).ToListAsync();
            if (identifier.Count == 0)
            {

                return 0;
            }
            return identifier.FirstOrDefault().Id;
        }

        //helper methods
        private string SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles");
            }
            var folderPath = Path.Combine(webRootPath, "UploadBill");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            //gets all files in source directory & delete previous ones except the last 24hours
            foreach (var fileItem in new DirectoryInfo(folderPath).GetFiles())
            {
                DateTime dt = File.GetCreationTime(fileItem.FullName);

                if (dt < DateTime.Today)
                {
                    File.Delete(fileItem.FullName);
                }
            }
            var fileslist = Directory.GetFiles(folderPath);
            file.CopyTo(stream);

            return filePath;
        }

        private async Task<bool> CheckIfExistsBybusinesstype(int organisationId, int propertyId, int customerId, int businesstypeId, int revenueId)
        {
            var bills = await _context.Billing.Where(x => x.CustomerId.Equals(customerId) && x.OrganisationId.Equals(organisationId) && x.PropertyId.Equals(propertyId)
                       && x.AppliedDate.Equals(DateTime.Now.Year.ToString())).ToListAsync();
            var response = bills.Where(a => a.BusinessTypeId.Equals(businesstypeId) && a.RevenueId.Equals(revenueId)).ToList();

            if (response.Count > 0)
            {
                return true;
            }
            return false;
        }
        //
        private async Task<int> CheckPropertyCustomer(int customerId, int propertyid)
        {
            var customerPropety = await _context.CustomerProperties.Where(x => x.CustomerId.Equals(customerId) && x.PropertyId.Equals(propertyid)).SingleOrDefaultAsync();

            if (customerPropety == null)
            {

                return 0;
            }
            return customerPropety.CustomerId;
        }
        private async Task<int> CheckBusinessType(int organisationId, string businessType)
        {
            var busType = await _context.BusinessTypes.Where(x => x.BusinessTypeName.Equals(businessType) && x.OrganisationId.Equals(organisationId)).SingleOrDefaultAsync();

            if (busType == null)
            {

                return 0;
            }
            return busType.Id;
        }
        // Get BusinessTypeId and return Id
        private async Task<int> GetBusinessTypeId(int organisationId, string businesssize)
        {
            var business = await _context.BusinessSizes.Where(x => x.OrganisationId.Equals(organisationId) && x.BusinessSizeName.Equals(businesssize)).FirstOrDefaultAsync();


            if (business == null)
            {

                return 0;
            }
            return business.Id;
        }
        //get revenueprice
        private async Task<decimal> GetRevenuePrice(int organisationId, string category, int businesssizeId, string payertype)
        {

            int payertypeId = 0;
            if (payertype.ToUpper() == "N") { payertypeId = 1; }
            else { payertypeId = 2; }
            var rev = await _context.Category
                                       .Join(_context.RevenuePrices,
                                        cat => cat.CategoryId,
                                        revprice => revprice.CategoryId,
                                        (cat, revprice) => new
                                        {
                                            Id = cat.CategoryId,
                                            price = revprice.Amount,
                                            categoryName = cat.CategoryName,
                                            businesssize = cat.BusinessSizeId,
                                            orgId = cat.OrganisationId,
                                            typeId = cat.PayerTypeId,
                                            Amount = revprice.Amount
                                        }).Where(x => x.categoryName.Equals(category) && x.typeId.Equals(payertypeId) && x.orgId.Equals(organisationId)).ToListAsync();
            var revprice = rev.Where(x => x.businesssize.Equals(businesssizeId)).ToList();

            if (revprice.Count == 0)
            {

                return 0;
            }
            return revprice.FirstOrDefault().Amount;
        }
        private async Task<int> CheckRevenue(int organisationId, string revenue)
        {
            var rev = await _context.Revenues.Where(x => x.RevenueCode.Equals(revenue) && x.OrganisationId.Equals(organisationId)).FirstOrDefaultAsync();

            if (rev == null)
            {

                return 0;
            }
            return rev.RevenueId;
        }
        private async Task<int> CheckAgency(string agencycode)
        {

            var agency = await _context.Agencies.Where(x => x.AgencyCode.Equals(agencycode)).ToListAsync();

            if (agency == null)
            {

                return 0;
            }
            return agency.FirstOrDefault().AgencyId;
        }
        private async Task<int> CheckCustomer(int organisationId, int propertyId, string createdby, string payerId, CreatePropertyBillUpload? createBill)
        {
            int customerId = 0;
            var customer = await _context.Customers.Where(x => x.FullName.Equals(createBill.FullName) && x.OrganisationId.Equals(organisationId)).SingleOrDefaultAsync();

            if (customer == null)
            {
                //create customer
                customerId = await CreateCustomer(organisationId, createdby, payerId, createBill);
            }

            var customerProperty = _context.CustomerProperties.Where(x => x.CustomerId.Equals(customerId) && x.PropertyId.Equals(propertyId)).SingleOrDefault();
            if (customerProperty == null)
            {
                CreateCustomerProperty(customerId, propertyId, organisationId, createdby);
            }




            return customerId;
        }
        private async void CreateCustomerProperty(int customerId, int propertyId, int organisationId, string createdby)
        {

            CustomerProperty customerPropertyEntity = new CustomerProperty();
            customerPropertyEntity.CreatedBy = createdby;
            customerPropertyEntity.DateCreated = DateTime.Now;
            customerId = customerId;
            _repository.CustomerProperty.CreateCustomerProperty(organisationId, propertyId, customerId, customerPropertyEntity);

        }
        private async Task<PreviewedbillResponse> CreatePayerId(CreatePropertyBillUpload createPropertyBill)
        {
            PreviewedbillResponse response = new PreviewedbillResponse();
            string payerId = "";

            if (createPropertyBill.PayerType.ToUpper() == "C")
            {
                CorporatePayerIDRequest customer = new CorporatePayerIDRequest();
                customer.Address = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName;
                customer.PhoneNumber = createPropertyBill.PhoneNumber;
                customer.Email = createPropertyBill.Email;
                customer.CompanyName = createPropertyBill.FullName;
                customer.DateofIncorporation = DateTime.Now.AddDays(-1000).ToString();

                //create corporate payerid
                var resp = CreateCorporatePID(customer);
                response.StatusMessage = resp.StatusMessage;

            }
            else if (createPropertyBill.PayerType.ToUpper() == "N")
            {
                //create individual payerId
                CustomerEnumerationDto customer = new CustomerEnumerationDto();
                customer.Type = "";
                customer.Title = "Mr";
                customer.Hash = "";
                customer.ClientId = "";
                customer.Sex = "M";
                customer.LastName = createPropertyBill.LastName;
                customer.FirstName = createPropertyBill.FirstName;
                customer.OtherName = createPropertyBill.MiddleName;
                customer.Address = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName;
                customer.MaritalStatus = "Single";
                customer.DateOfBirth =
                customer.Phone = createPropertyBill.PhoneNumber;
                customer.Email = createPropertyBill.Email;
                customer.State = "State";
                var resp = CreatePIDWithBioData(customer);
            }
            else
            {
                response.Bill = createPropertyBill;
                response.StatusMessage = "Bill already exists";

            }
            return response;
        }
        private Response CreateCorporatePID(CorporatePayerIDRequest customer)
        {
            var pidEntity = _mapper.Map<CorporatePayerIDRequest>(customer);
            GenerateCorporatePID = _modelService.CreateCorporatePID(pidEntity);

            CorporatePayerIDResponse payerIDResponse = new();

            foreach (var response in GenerateCorporatePID)
            {
                payerIDResponse.Flag = response.Flag;
                payerIDResponse.outData = response.outData;
            }

            var payerIDResponseEntity = _mapper.Map<CorporatePayerIDResponse>(payerIDResponse);

            Response dataResponse = new();

            //dataResponse.StatusMessage = ;
            dataResponse.Data = payerIDResponseEntity;
            dataResponse.Status = 200;

            return dataResponse;
        }
        private async Task<Response> CreatePIDWithBioData(CustomerEnumerationDto customer)
        {
            var pidEntity = _mapper.Map<CustomerEnumerationDto>(customer);

            var pidCreationUrl = $"{_pidConfig.BASE_URL}/Interface/pidcreation";
            var pidCreationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + customer.Phone + customer.Email + customer.Address + _pidConfig.STATE);

            //map hash, state and client id
            pidEntity.Hash = pidCreationHash;
            pidEntity.State = _pidConfig.STATE;
            pidEntity.ClientId = _pidConfig.CLIENT_ID;

            //generate PID
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, pidCreationUrl);

            //serialize payload 
            var payload = JsonConvert.SerializeObject(pidEntity);
            var content = new StringContent(payload, null, "application/json");
            request.Content = content;

            //get response
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

            //deserialise response
            PIDResponse output = JsonConvert.DeserializeObject<PIDResponse>(responseContent);
            Response dataResponse = new();

            if (output.Status == "FAILED")
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Data = output;
                dataResponse.Status = 401;
            }

            else
            {
                dataResponse.StatusMessage = output.Status;
                dataResponse.Status = 200;
                dataResponse.Data = output;
            }

            return dataResponse;
        }

        private async Task<PreviewedbillResponse> CheckIfPropertyExists(int organisationId, int agencyId, CreatePropertyBillUpload createPropertyBill)
        {
            PreviewedbillResponse response = new PreviewedbillResponse();
            Property property = new Property();
            SpaceIdentifier spaceIdentifier = new SpaceIdentifier();
           // var wardId = await _context.Wards.Where(x => x.WardName.Equals(createPropertyBill.Ward) && x.OrganisationId.Equals(organisationId)).SingleOrDefaultAsync();
       
               // property.WardId = wardId.Id;
                spaceIdentifier = await _context.SpaceIdentifiers.Where(x => x.SpaceIdentifierName.Equals(createPropertyBill.SpaceIdentifier) && x.OrganisationId.Equals(organisationId)).SingleOrDefaultAsync();
                if (spaceIdentifier != null)
                {
                    property.SpaceIdentifierId = spaceIdentifier.Id;
                    response.Data = property;
                    var properties = await _context.Properties.Where(x => x.AgencyId.Equals(agencyId)  && x.OrganisationId.Equals(organisationId)).ToListAsync();
                    if (properties.Any())
                    {
                        var address = createPropertyBill.BuildingNumber + " " + createPropertyBill.StreetName;
                        var result = properties.Where(x => x.BuildingNo.Equals(Int32.Parse(createPropertyBill.BuildingNumber)) && x.BuildingName.Equals(createPropertyBill.BuildingName) && x.SpaceIdentifierId.Equals(spaceIdentifier.Id)).ToList();
                        var result1 = result.Where(x => x.SpaceFloor.Equals(Int32.Parse(createPropertyBill.SpaceFloor)) && x.LocationAddress.Equals(address)).SingleOrDefault();
                        if (result1 != null)
                        {
                            property = result1;

                        }
                    }

            
            }
            response.Data = property;
            //if (wardId == null)
            //{
            //    response.StatusMessage = "Ward does not exist";
            //    response.Status = 404;
            //    response.Bill = createPropertyBill;

            //}
            if (spaceIdentifier != null)
            {

                response.StatusMessage = "SpaceIdentifier does not exist";
                response.Status = 404;
                response.Bill = createPropertyBill;

            }
            return response;
        }
        private async Task CheckIfOrganisationExists(int organisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(organisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", organisationId);
        }

    }
}