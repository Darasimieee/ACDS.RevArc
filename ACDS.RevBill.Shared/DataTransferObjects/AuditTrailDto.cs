using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public record AuditTrailDto(string EventType, string IpAddress, string RequestUrl, string UserName, string MachineName, int ResponseStatusCode,
        DateTime InsertedDate);
}