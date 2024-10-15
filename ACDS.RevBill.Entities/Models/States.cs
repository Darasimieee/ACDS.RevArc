using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class States
	{
        [Key]
        public int Id { get; set; }
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
        public int? CountryId { get; set; }
        public string? Capital { get; set; }
        public string? CreatedBy { get; set; }
        public int RecordStatus { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}

