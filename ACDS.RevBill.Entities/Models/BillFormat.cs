using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class BillFormat : EntityBase
    {
        [Key]
        public int BillFormatId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public string SignatureOneName { get; set; }
        public byte[] SignatureOneData { get; set; }
        public string? SignatureTwoName { get; set; }
        public byte[]? SignatureTwoData { get; set; }
        public string ContentOne { get; set; }
        public string? ContentTwo { get; set; }
        public string? ClosingSection { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}