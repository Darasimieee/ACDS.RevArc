using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class RevenueCategories : EntityBase
	{
        [Key]
        public int RevenueCategoryId { get; set; }
        public int OrganisationId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        [ForeignKey(nameof(Revenues))]
        public int RevenueId { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

       
    }
}

