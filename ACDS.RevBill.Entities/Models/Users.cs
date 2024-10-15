using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ForeignKey("Agencies")]
        public int AgencyId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool AccountConfirmed { get; set; }        
        public string? PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool Active { get; set; }
        public int AccessFailedCount { get; set; }
        public string? TenantName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        public Agencies? Agencies { get; set; }
    }
}


	