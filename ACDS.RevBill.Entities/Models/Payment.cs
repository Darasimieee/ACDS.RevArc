using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Entities.Models
{
    public class Payment
	{
        [Key]
        public long PaymentId { get; set; }
        public int PayerId { get; set; }
        public int EntryId { get; set; }
        public string WebGuid { get; set; }
        public string AssessRef { get; set; }
        public DateTime EntryDate { get; set; }
        public string PayerType { get; set; }
        public string Agency { get; set; }
        public string Revenue { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        [ForeignKey(nameof(BankCode))]
        public string BankCode { get; set; }
        [Precision(18, 2)]
        public decimal BankAmount { get; set; }
        public DateTime BankEntryDate { get; set; }
        public int BankTransId { get; set; }

        public string BankTranRef { get; set; }
    }
}