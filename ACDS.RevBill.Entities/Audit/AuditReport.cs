using System;
namespace ACDS.RevBill.Entities.Models
{
	public class AuditReport
	{
        public string? EventType { get; set; }
        public string? IpAddress { get; set; }
        public string? RequestUrl { get; set; }
        public string? UserName { get; set; }
        public string? MachineName { get; set; }
        public int ResponseStatusCode { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}

