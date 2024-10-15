using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class BusinessProfile : EntityBase
    {
        [Key]
        public int BusinessProfileId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }
        [ForeignKey("BusinessType")]
        public int BusinessTypeId { get; set; }
        [ForeignKey("BusinessSize")]
        public int BusinessSizeId { get; set; }
        [ForeignKey("Revenues")]
        public int RevenueId { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public Property? Property { get; set; } 
        public BusinessType? BusinessType { get; set; }
        public BusinessSize? BusinessSize { get; set; }
        public Revenues? Revenues { get; set; }
        public Customers? Customers { get; set; }
    }
}
