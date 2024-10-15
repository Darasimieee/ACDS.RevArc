using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Exceptions;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Payment;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Services
{
    internal sealed class PaymentService : IPaymentService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private DataContext _context;
        private readonly int _maxFileSize = 200000; //in bytes
        private readonly string[] _extensions = { ".jpg", ".png", ".jpeg" };
        public string? LogoFileName { get; set; }
        public string? LogoFileExtension { get; set; }
        public string? LogoNewFileName { get; set; }

        public PaymentService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, DataContext context)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<(IEnumerable<GetPaymentDto> payment, MetaData metaData)> GetOrganisationPaymentsAsync(int organisationId,
            PaymentParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var paymentsWithMetaData = await _repository.Payment.GetAllPaymentsAsync(organisationId, requestParameters, trackChanges);

            var paymentDto = _mapper.Map<IEnumerable<GetPaymentDto>>(paymentsWithMetaData);

            return (payment: paymentDto, metaData: paymentsWithMetaData.MetaData);
        }
        public async Task<List<PaymentByAgency>> GetOrganisationPaymentByAgencyAsync(int organisationId)
        {
            var summaryList = await _context.Payments
        .Join(
                _context.Billing,
                payment => payment.WebGuid,
                billing => billing.BillReferenceNo,
                (payment, billing) => new
                {
                    Agency = payment.Agency,
                    bill = billing.BillAmount,
                    Year = billing.Year,
                    amount = payment.Amount,
                }
                    )
        .Where(a => a.Year == DateTime.Now.Year)
        .GroupBy(a=> a.Agency)
        .Select(group => new
        {
            Id = 0,
            Agency = group.Key,
            Totalbill = group.Sum(item => item.bill),
            BillCount = group.Count(),
            TotalbillPaid = group.Sum(item => item.amount),
            BillOutstanding = group.Sum(item => item.bill) - group.Sum(item => item.amount)
        }).ToListAsync();

            List<PaymentByAgency> result = new List<PaymentByAgency>();
            foreach (var item in summaryList)
            {
                string formattedTotalbill = item.Totalbill.ToString("#,##0.00");

                string formattedBillPaid = item.TotalbillPaid.ToString("#,##0.00");
                string formattedBillOutstanding = item.BillOutstanding.ToString("#,##0.00");

                result.Add(new PaymentByAgency
                {
                    Id = 0,
                    AgencyName = item.Agency,
                    BillValue = formattedTotalbill,
                    BillPaid = formattedBillPaid,
                    BillCount = item.BillCount,
                    BillOutstanding = formattedBillOutstanding
                });
            }

            // set the Id field to an auto-incrementing value
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id = i + 1;
            }

            return result;
        }

        public async Task<List<DailyPaymentbyBankDto>> GetOrganisationDailyPaymentByBankAsync(int organisationId)
        {
            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            var summaryList = await _context.Payments
        .Join(
                _context.Bank_Code,
                payment => payment.BankCode,
                bank => bank.BankCode,
                (payment, billing) => new
                {
                    BankCode = payment.BankCode,
                    Date = payment.BankEntryDate,
                    Amount = payment.Amount,
                }
               )
        .Where(a => a.Date >= startOfDay && a.Date <= endOfDay)
        .GroupBy(a => a.BankCode)
        .Select(a => new
        {
            BankName = _context.Bank_Code.Where(e => e.BankCode.Equals(a.SingleOrDefault().BankCode)).SingleOrDefault().BankName,
            Amount = a.Sum(item => item.Amount)
        }).ToListAsync();

            List<DailyPaymentbyBankDto> result = new List<DailyPaymentbyBankDto>();
            foreach (var item in summaryList)
            {
                result.Add(new DailyPaymentbyBankDto
                {
                  
                    BankName = item.BankName,
                    Payment = item.Amount.ToString("#,##0.00")

                });
            }

            //// set the Id field to an auto-incrementing value
            //for (int i = 0; i < result.Count; i++)
            //{
            //    result[i].Id = i + 1;
            //}

            return result;
        }

        public async Task<List<PaymentbyRevenueDto>> GetOrganisationPaymentByRevenueAsync(int organisationId)
        {
            var summaryList = await _context.Payments
        .Join(
                _context.Billing,
                payment => payment.WebGuid,  
                billing => billing.BillReferenceNo, 
                (payment, billing) => new      
                {
                    Revenue=payment.Revenue,
                    bill = billing.BillAmount,
                    Year = billing.Year,
                    amount = payment.Amount,
                }
                    )
        .Where(a => a.Year == DateTime.Now.Year)
        .GroupBy(a=>a.Revenue)
        .Select(group => new
                {
                    Id = 0,
                    RevenueName = group.Key,
                    Totalbill = group.Sum(item => item.bill),
                    BillCount = group.Count(),
                    TotalbillPaid = group.Sum(item => item.amount),
                    BillOutstanding = group.Sum(item => item.bill)- group.Sum(item => item.amount)
        }).ToListAsync();

    List<PaymentbyRevenueDto> result = new List<PaymentbyRevenueDto>();
            foreach (var item in summaryList)
            {
                string formattedTotalbill = item.Totalbill.ToString("#,##0.00");

                string formattedBillPaid = item.TotalbillPaid.ToString("#,##0.00");
                string formattedBillOutstanding = item.BillOutstanding.ToString("#,##0.00");
                result.Add(new PaymentbyRevenueDto
                {
                    RevenueName = item.RevenueName,
                    BillValue   = formattedTotalbill,
                    BillPaid = formattedBillPaid,
                    BillCount=item.BillCount,
                    BillOutstanding= formattedBillOutstanding
                });
            }

            // set the Id field to an auto-incrementing value
            for (int i = 0; i < result.Count; i++)
{
    result[i].Id = i + 1;
}

return result;
        }
        public async Task<List<PaymentbyBankDto>> GetOrganisationPaymentByBankAsync(int organisationId)
        {
            var summaryList = await _context.Payments
        .Join(
                _context.Billing,
                payment => payment.WebGuid,
                billing => billing.BillReferenceNo,
                (payment, billing) => new
                {
                    BankCode = payment.BankCode,
                    bill = billing.BillAmount,
                    Year = billing.Year,
                    amount = payment.Amount,
                }
                    )
        .Where(a => a.Year ==DateTime.Now.Year)
        .GroupBy(a => new { a.BankCode })
        .Select(group => new
        {
            Id = 0,
            BankName = _context.Bank_Code.Where(e => e.BankCode.Equals(group.Key.BankCode)).SingleOrDefault().BankName,
            Totalbill= group.Sum(item => item.bill),
            BillCount = group.Count(),
            TotalbillPaid = group.Sum(item => item.amount)

        }).ToListAsync();          

            
            List<PaymentbyBankDto> result = new List<PaymentbyBankDto>();
            foreach (var item in summaryList)
            {
                string formattedTotalbill = item.Totalbill.ToString("#,##0.00");

                string formattedBillPaid = item.TotalbillPaid.ToString("#,##0.00");

                result.Add(new PaymentbyBankDto
                {
                    Id =0,
                    BankName = item.BankName,
                    BillValue = formattedTotalbill,
                    BillCount = item.BillCount,
                    BillPaid = formattedBillPaid
                });
            }

            // set the Id field to an auto-incrementing value
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Id = i + 1;
            }

            return result;
        }

        public async Task<GetPaymentDto> GetPaymentInOrganisationAsync(int organisationId, long paymentId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var paymentDb = await CheckIfPaymentExists(organisationId, paymentId, trackChanges);

            var payment = _mapper.Map<GetPaymentDto>(paymentDb);

            return payment;
        }

        public async Task<Response> GetAllIndividualPaymentHistoriesAsync(int userId)
        {
            Response dataResponse = new Response();
            List<GetPaymentDto> result = new List<GetPaymentDto>();

            var user = await _context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            var payerId = await _context.Customers.Where(x => x.Email == user.Email).Select(x => x.PayerId).FirstOrDefaultAsync();
            //return message if customerId does not exist
            if (payerId is null)
            {
                dataResponse.StatusMessage = "This User ID does not exist";
                dataResponse.Status = 404;
                dataResponse.Data = result;

                return dataResponse;
            }

            //get digit from payer id
            string digit = new string(payerId.Where(char.IsDigit).ToArray());

            //get payment details
            var paymentRecord = await _context.Payments.Where(x => x.PayerId.ToString() == digit)
                                     .Select(a => new
                                     {
                                         Payment = a,
                                         Bill = _context.Billing.Where(p => p.BillReferenceNo == a.WebGuid).FirstOrDefault(),
                                     }).ToListAsync();

            //add to list
            foreach (var item in paymentRecord)
            {
                result.Add(new GetPaymentDto
                {
                    PaymentId = item.Payment.PaymentId,
                    Amount = item.Payment.Amount,
                    WebGuid = item.Payment.WebGuid,
                    PayerId = item.Payment.PayerId,
                    EntryId = item.Payment.EntryId,
                    AssessRef = item.Payment.AssessRef,
                    EntryDate = item.Payment.EntryDate,
                    PayerType = item.Payment.PayerType,
                    Agency = item.Payment.Agency,
                    Revenue = item.Payment.Revenue,
                    BankAmount = item.Payment.BankAmount,
                    BankEntryDate = item.Payment.BankEntryDate,
                    BankTransId = item.Payment.BankTransId,
                    BankTranRef = item.Payment.BankTranRef,

                });
            }

            if (paymentRecord.Count() == 0)
            {
                dataResponse.StatusMessage = "No Payment Record Found";
                dataResponse.Status = 200;
                dataResponse.Data = result;
            }

            else
            {
                dataResponse.StatusMessage = "Payment Record Found";
                dataResponse.Status = 200;
                dataResponse.Data = result;
            }

            return dataResponse;
        }

        public async Task<Response> GetIndividualPaymentHistoryAsync(int userId, long paymentId)
        {
            Response dataResponse = new Response();
            List<GetPaymentDto> result = new List<GetPaymentDto>();

            var user = await _context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            var payerId = await _context.Customers.Where(x => x.Email == user.Email).Select(x => x.PayerId).FirstOrDefaultAsync();

            //return message if customerId does not exist
            if (payerId is null)
            {
                dataResponse.StatusMessage = "This User ID does not exist";
                dataResponse.Status = 404;
                dataResponse.Data = result;

                return dataResponse;
            }

            //get digit from payer id
            string digit = new string(payerId.Where(char.IsDigit).ToArray());

            //get payment details
            var paymentRecord = await _context.Payments.Where(x => x.PayerId.ToString() == digit && x.PaymentId.Equals(paymentId))
                                     .Select(a => new
                                     {
                                         Payment = a,
                                         Bill = _context.Billing.Where(p => p.BillReferenceNo == a.WebGuid).FirstOrDefault(),
                                     }).ToListAsync();

            //add to list
            foreach (var item in paymentRecord)
            {
                result.Add(new GetPaymentDto
                {
                    PaymentId = item.Payment.PaymentId,
                    Amount = item.Payment.Amount,
                    WebGuid = item.Payment.WebGuid,
                    PayerId = item.Payment.PayerId,
                    EntryId = item.Payment.EntryId,
                    AssessRef = item.Payment.AssessRef,
                    EntryDate = item.Payment.EntryDate,
                    PayerType = item.Payment.PayerType,
                    Agency = item.Payment.Agency,
                    Revenue = item.Payment.Revenue,
                    BankAmount = item.Payment.BankAmount,
                    BankEntryDate = item.Payment.BankEntryDate,
                    BankTransId = item.Payment.BankTransId,
                    BankTranRef = item.Payment.BankTranRef,

                });
            }


            if (paymentRecord == null)
            {
                dataResponse.StatusMessage = "No Payment Record Found";
                dataResponse.Status = 200;
                dataResponse.Data = result;
            }

            else
            {
                dataResponse.StatusMessage = "Payment Record Found";
                dataResponse.Status = 200;
                dataResponse.Data = result;
            }

            return dataResponse;
        }

        public async Task<Response> AddPaymentHistoryAsync(CreatePaymentDto payment)
        {
            Response dataResponse = new Response();

            var paymentEntity = _mapper.Map<Payment>(payment);

            //update the bill status
            var getBill = await _context.Billing.Where(x => x.BillReferenceNo == paymentEntity.WebGuid).FirstOrDefaultAsync();
            
            //if the amount paid is zero, update the value with the payment amount
            if (getBill.AmountPaid == 0)
            {
                //update amount paid on bill
                getBill.AmountPaid = paymentEntity.Amount;
            }

            //if the amount paid is not zero, sum the value with the payment amount
            else if (getBill.AmountPaid != 0)
            {
                //update amount paid on bill
                getBill.AmountPaid += paymentEntity.Amount;
            }

            //if bill amount is same as amount paid, update to fully paid
            if (getBill.BillAmount == getBill.AmountPaid)
            {
                getBill.BillStatusId = 3;
            }

            //if bill amount is not equal to amount paid
            if (getBill.BillAmount != getBill.AmountPaid)
            {
                getBill.BillStatusId = 2;
            }

            //update bill arrears
            getBill.BillArrears = getBill.BillAmount - getBill.AmountPaid;
          
            //check if payment exists already
            bool alreadyExists = _context.Payments.Any(x => x.WebGuid == paymentEntity.WebGuid && x.BankTransId == paymentEntity.BankTransId);
            if (alreadyExists)
            {
                dataResponse.StatusMessage = "Payment record already exists";
                dataResponse.Status = 400;
                return dataResponse;
            }

            // If payment doesn't exist, proceed with saving
            await _context.SaveChangesAsync();

            _repository.Payment.CreatePayment(paymentEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Payment added successfully";
            dataResponse.Status = 200;
            dataResponse.Data = paymentEntity;
            _context.Billing.Update(getBill);
            _context.SaveChanges(); 
            return dataResponse;
        }

        public async Task<(IEnumerable<GetPaymentGatewayDto> banks, MetaData metaData)> GetAllPaymentGatewaysAsync(DefaultParameters requestParameters, bool trackChanges)
        {
            var banksWithMetaData = await _repository.Bank.GetAllBanksAsync(requestParameters, trackChanges);

            var bankDto = _mapper.Map<IEnumerable<GetPaymentGatewayDto>>(banksWithMetaData);

            return (banks: bankDto, metaData: banksWithMetaData.MetaData);
        }

        public async Task<GetPaymentGatewayDto> GetPaymentGatewayAsync(int id, bool trackChanges)
        {
            var paymentGateway = await _repository.Bank.GetBankAsync(id, trackChanges);
            if (paymentGateway is null)
                throw new IdNotFoundException("Payment Gateway", id);

            var bankDto = _mapper.Map<GetPaymentGatewayDto>(paymentGateway);

            return bankDto;
        }

        public async Task<Response> AddPaymentGatewayAsync(CreatePaymentGatewayDto bank)
        {
            Response dataResponse = new Response();

            var bankEntity = _mapper.Map<Banks>(bank);

            //create file attachment for logo
            if (bank.BankLogo != null)
            {
                if (bank.BankLogo.Length > 0)
                {
                    LogoFileName = Path.GetFileName(bank.BankLogo.FileName);
                    LogoFileExtension = Path.GetExtension(LogoFileName);
                    LogoNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension);
                }

                if (bank.BankLogo.Length > _maxFileSize)
                {
                    dataResponse.StatusMessage = "Maximum allowed image size is 200kb";
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
                    bank.BankLogo.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    bankEntity.BankLogoData = fileBytes;
                    bankEntity.BankLogoName = bank.BankLogo.FileName;
                }
            }

            _repository.Bank.CreateBank(bankEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Payment Gateway Created Successfully";
            dataResponse.Status = 200;
            dataResponse.Data = bankEntity;

            return dataResponse;
        }

        public async Task<(IEnumerable<GetOrganisationPaymentGatewayDto> banks, MetaData metaData)> GetAllOrganisationPaymentGatewaysAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var organisationBanksWithMetaData = await _repository.OrganisationBank.GetAllOrganisationBanksAsync(organisationId, requestParameters, trackChanges);

            var organisationBankDto = _mapper.Map<IEnumerable<GetOrganisationPaymentGatewayDto>>(organisationBanksWithMetaData);

            return (banks: organisationBankDto, metaData: organisationBanksWithMetaData.MetaData);
        }

        public async Task<GetOrganisationPaymentGatewayDto> GetOrganisationPaymentGatewayAsync(int organisationId, int organisationBankId, bool trackChanges)
        {
            await CheckIfOrganisationExists(organisationId, trackChanges);

            var organisationBank = await _repository.OrganisationBank.GetOrganisationBankAsync(organisationId, organisationBankId, trackChanges);
            if (organisationBank is null)
                throw new IdNotFoundException("Organisation Bank", organisationBankId);

            var organisationBankDto = _mapper.Map<GetOrganisationPaymentGatewayDto>(organisationBank);

            return organisationBankDto;
        }

        public async Task<Response> AddPaymentGatewayToOrganisationAsync(int organisationId, IEnumerable<CreateOrganisationPaymentGatewayDto> banks)
        {
            Response dataResponse = new Response();

            if (banks.Count() == 0)
            {
                dataResponse.StatusMessage = "Request body cannot be empty";
                dataResponse.Status = 400;
                return dataResponse;
            }

            foreach (var bank in banks)
            {
                // check if paymentGatewayId exists
                var paymentGateway = await _context.Banks.ToListAsync();
                bool checkIfPaymentGatewayExists = paymentGateway.Any(x => x.BankId == bank.BankId);
                if (!checkIfPaymentGatewayExists)
                    throw new IdNotFoundException("Payment Gateway", bank.BankId);

                // Check if the paymentGatewayId already exists in organisation banks table
                var organisationBanks = await _context.OrganisationBanks.ToListAsync();
                bool paymentGatewayExists = organisationBanks.Any(x => x.BankId == bank.BankId);
                if (paymentGatewayExists)
                {
                    dataResponse.StatusMessage = "Payment Gateway Already Exists";
                    dataResponse.Status = 400;
                    return dataResponse;
                }

                var bankEntity = _mapper.Map<OrganisationBanks>(bank);
                bankEntity.BankStatus = true;

                _repository.OrganisationBank.CreateOrganisationBank(organisationId, bank.BankId, bankEntity);
            }

            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Payment Gateways Added to Organisation Successfully";
            dataResponse.Status = 200;
            dataResponse.Data = banks;

            return dataResponse;
        }

        public async Task<Response> UpdatePaymentGatewayAsync(int id, UpdatePaymentGatewayDto paymentGateway, bool trackChanges)
        {
            Response dataResponse = new Response();

            var paymentGatewayEntity = await _repository.Bank.GetBankAsync(id, trackChanges);
            if (paymentGatewayEntity is null)
                throw new IdNotFoundException("Payment Gateway", id);

            //create file attachment for logo
            if (paymentGateway.BankLogo != null)
            {
                if (paymentGateway.BankLogo.Length > 0)
                {
                    LogoFileName = Path.GetFileName(paymentGateway.BankLogo.FileName);
                    LogoFileExtension = Path.GetExtension(LogoFileName);
                    LogoNewFileName = string.Concat(Convert.ToString(Guid.NewGuid()), LogoFileExtension);
                }

                if (paymentGateway.BankLogo.Length > _maxFileSize)
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
                    paymentGateway.BankLogo.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    //map logo data
                    paymentGatewayEntity.BankLogoData = fileBytes;
                    paymentGatewayEntity.BankLogoName = paymentGateway.BankLogo.FileName;
                }
            }

            _mapper.Map(paymentGateway, paymentGatewayEntity);
            await _repository.SaveAsync();

            dataResponse.StatusMessage = "Payment Gateway Successfully Updated";
            dataResponse.Status = 200;
            dataResponse.Data = paymentGatewayEntity;

            return dataResponse;
        }

        public async Task<Response> UpdatePaymentGatewayForOrganisationAsync(int organisationId, int paymentGatewayId, UpdateOrganisationPaymentGatewayDto paymentGateway, bool trackChanges)
        {
            Response dataResponse = new Response();

            await CheckIfOrganisationExists(organisationId, trackChanges);

            var paymentGatewayEntity = await _repository.OrganisationBank.GetOrganisationBankAsync(organisationId, paymentGatewayId, trackChanges);
            if (paymentGatewayEntity is null)
                throw new IdNotFoundException("Payment Gateway", paymentGatewayId);

            _mapper.Map(paymentGateway, paymentGatewayEntity);
            await _repository.SaveAsync();
            
            dataResponse.StatusMessage = "Organisation Payment Gateway Successfully Updated";
            dataResponse.Status = 200;
            dataResponse.Data = paymentGatewayEntity;

            return dataResponse;
        }

        //helper methods
        private async Task CheckIfOrganisationExists(int organisationId, bool trackChanges)
        {
            var company = await _repository.Organisation.GetOrganisationAsync(organisationId, trackChanges);
            if (company is null)
                throw new IdNotFoundException("organisation", organisationId);
        }
        public async Task<Response> TotalPaymentsThisYear(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var currentTime = DateTime.Now;
            var startOfYear = new DateTime(currentTime.Year, 1, 1);

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalAmount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfYear && x.EntryDate <= currentTime)
                .Sum(x => x.Amount);

            string formattedNumber = totalAmount.ToString("#,##0.00");

            dataResponse.StatusMessage = "Total Payments this Month";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;
        }
        public async Task<Response> TotalPaymentsThisMonth(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalAmount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfMonth && x.EntryDate <= endOfMonth) 
                .Sum(x => x.Amount);

            string formattedNumber = totalAmount.ToString("#,##0.00");

            dataResponse.StatusMessage = "Total Payments this Month";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;
        }

        public async Task<Response> TotalPaymentsThisWeek(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalAmount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfWeek && x.EntryDate <= endOfWeek)
                .Sum(x => x.Amount);

            string formattedNumber = totalAmount.ToString("#,##0.00");

            dataResponse.StatusMessage = "Total Payments this Week";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;
        }

        public async Task<Response> TotalPaymentsToday(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalAmount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfDay && x.EntryDate <= endOfDay)
                .Sum(x => x.Amount);

            string formattedNumber = totalAmount.ToString("#,##0.00");

            dataResponse.StatusMessage = "Total Payments Today";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;
        }

        public async Task<Response> TotalCountOfPaymentsThisMonth(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var currentMonth = DateTime.Now;
            var startOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Set end of month to last tick of the previous month

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalCount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfMonth && x.EntryDate <= endOfMonth)
                .Count();

            string formattedNumber = totalCount.ToString("N0");

            dataResponse.StatusMessage = "Total Payment Count this Month";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;
        }

        public async Task<Response> TotalCountOfPaymentsThisWeek(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Set start of week to Sunday
            var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Set end of week to last tick of Saturday

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalCount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfWeek && x.EntryDate <= endOfWeek)
                .Count();

            string formattedNumber = totalCount.ToString("N0");

            dataResponse.StatusMessage = "Total Payment Count this Week";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;

        }

        public async Task<Response> TotalCountOfPaymentsToday(int organisationId)
        {
            Response dataResponse = new Response();

            //check if organisation exists
            bool organisationExists = _context.Organisations.Any(x => x.OrganisationId == organisationId);
            if (organisationExists == false)
            {
                dataResponse.StatusMessage = "Organisation ID not found";
                dataResponse.Status = 404;

                return dataResponse;
            }

            string organisationIdString = organisationId.ToString();

            // Get the value after the zero in the organisationId
            string organisationIdAfterZero = organisationIdString.Substring(organisationIdString.IndexOf('0') + 1);

            var today = DateTime.Today;
            var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Set end of day to last tick of previous day

            // Fetch all Payments records from the database
            var paymentRecords = await _context.Payments.ToListAsync();

            var totalCount = paymentRecords
                .Where(x => x.WebGuid.Substring(x.WebGuid.IndexOf('0') + 1) == organisationIdAfterZero && x.EntryDate >= startOfDay && x.EntryDate <= endOfDay)
                .Count();

            string formattedNumber = totalCount.ToString("N0");

            dataResponse.StatusMessage = "Total Payment Count Today";
            dataResponse.Status = 200;
            dataResponse.Data = formattedNumber;

            return dataResponse;
        }

        //helper function
        private async Task<Payment> CheckIfPaymentExists(int organisationId, long paymentId, bool trackChanges)
        {
            var paymentDb = await _repository.Payment.GetPaymentAsync(organisationId, paymentId, trackChanges);
            if (paymentDb is null)
                throw new IdNotFoundException("payment", (int)paymentId);

            return paymentDb;
        }
    }
}