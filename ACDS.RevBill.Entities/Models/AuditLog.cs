using System;

namespace ACDS.RevBill.Entities.Models
{
	public class AuditLog : EntityBase
	{
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Details { get; set; }
    }
}