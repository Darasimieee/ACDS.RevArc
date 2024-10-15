using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class Streets : EntityBase
    {
        [Key]
        public int StreetId { get; set; }
        [ForeignKey("Agencies")]
        public int AgencyId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }

        ////[ForeignKey("Ward")]
        // public int? WardId { get; set; }
        //public Ward? Ward { get; set; }
        public string? StreetName { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }  
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

    }
}