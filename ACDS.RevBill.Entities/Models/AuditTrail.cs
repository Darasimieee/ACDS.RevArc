using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class AuditTrail : EntityBase
	{
        [Key]
        public long EventId { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string? LastUpdatedDate { get; set; }
        public string? JsonData { get; set; }
        public string? EventType { get; set; }
        public string? User { get; set; }
    }
}

