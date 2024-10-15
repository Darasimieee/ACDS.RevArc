using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Entities.Models
{
	public class RevenuePrices : EntityBase
	{
        [Key]
        public int RevenuePriceId { get; set; }
        public int OrganisationId { get; set; }
        //[ForeignKey(nameof(Agencies))]
        //public int AgencyId { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        [ForeignKey(nameof(Revenues))]
        public int RevenueId { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }  
    }
}

