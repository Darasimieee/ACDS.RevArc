using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Revenues : EntityBase
	{
        [Key]
        public int RevenueId { get; set; }
        [ForeignKey(nameof(BusinessType))]
        public int BusinessTypeId { get; set; }        
        public int OrganisationId { get; set; }
        public string? RevenueCode { get; set; }
        public string? RevenueName { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}