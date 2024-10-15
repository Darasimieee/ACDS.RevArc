using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class EmailAccounts : EntityBase
	{
        [Key]
        public int EmailAccountId { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? From { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? SmtpServer { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool EnableSSL { get; set; }
        public bool EmailAccountStatus { get; set; } 
        public bool UseDefaultCredentials { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
    }
}

