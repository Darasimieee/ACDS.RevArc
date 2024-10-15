using System;
using ACDS.RevBill.Contracts;
using Microsoft.Extensions.Logging;

//using NLog;
using NPOI.SS.Formula.Functions;

namespace ACDS.RevBill.Services
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger _logger;

        // private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager(ILoggerFactory loggerfactory)
        {
           _logger = loggerfactory.CreateLogger("revbill");
        }

        public void LogDebug(string message) => _logger.LogDebug(message);

        public void LogError(string message) => _logger.LogError(message);

        public void LogInfo(string message) => _logger.LogInformation(message);

        public void LogWarn(string message) => _logger.LogWarning(message);
    }
}

