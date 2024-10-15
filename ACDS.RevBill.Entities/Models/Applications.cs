using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Applications
	{
        [Key]
        public int ApplicationId { get; set; }
        public string? ApplicationName { get; set; }
        public string? ApplicationUrl { get; set; }
        public byte[]? Logo { get; set; }
        public int ApplicationCode { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

