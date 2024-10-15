using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class PasswordHistory
	{
        [Key]
        public int PasswordHistoryId { get; set; }
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        public string? Password { get; set; }
        public string? TenantName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}

