using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Banks
	{
        [Key]
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankUrl { get; set; }
        public string BankDescription { get; set; }
        public string? BankLogoName { get; set; }
        public byte[]? BankLogoData { get; set; }
        public bool BankStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}