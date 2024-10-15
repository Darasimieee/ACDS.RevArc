using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class HeadRevenue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HeadRevenueId { get; set; }

        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public string AgencyHead { get; set; }
        public string? RevenueCode { get; set; }
        public string? RevenueName { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}