using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.DataTransferObjects;
using ACDS.RevBill.Shared.RequestFeatures;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static ACDS.RevBill.Entities.Audit.AuditResponse;

namespace ACDS.RevBill.Services
{
    internal sealed class AuditTrailService : IAuditTrailService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private DataContext _context;

        public AuditTrailService(ILoggerManager logger, IMapper mapper, DataContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<AuditTrailDto>> GetAuditTrailAsync(PaginationFilter filter)
        {
            //declare list to store audit response
            List<AuditReport> auditTrail = new List<AuditReport>();
            List<string?>? auditJsonData = new List<string?>();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            //fetch global configuration from db
            auditJsonData = await _context.AuditTrail
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Select(y => y.JsonData)
                .ToListAsync();

            foreach (var x in auditJsonData)
            {
                AuditReport deserializeAuditLog = new AuditReport();

                //deserialize audit json data
                Root data = JsonConvert.DeserializeObject<Root>(x);

                deserializeAuditLog.IpAddress = data.Action.IpAddress;
                deserializeAuditLog.EventType = data.EventType;
                deserializeAuditLog.InsertedDate = data.StartDate;
                deserializeAuditLog.RequestUrl = data.Action.RequestUrl;
                deserializeAuditLog.ResponseStatusCode = data.Action.ResponseStatusCode;
                deserializeAuditLog.UserName = data.Environment.UserName;
                deserializeAuditLog.MachineName = data.Environment.MachineName;

                auditTrail.Add(deserializeAuditLog);
            }

            var auditDto = _mapper.Map<IEnumerable<AuditTrailDto>>(auditTrail);

            return auditDto;
        }
    }
}