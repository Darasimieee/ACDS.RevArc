using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class Property : EntityBase
    {
        [Key]
        public int PropertyId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ForeignKey("Agencies")]
        public int AgencyId { get; set; }

        [ForeignKey("Streets")]
        public int StreetId { get; set; }

        [ForeignKey("SpaceIdentifier")]
        public int SpaceIdentifierId { get; set; }
        
       
        public string? LocationAddress { get; set; }
        public int SpaceFloor { get; set; }
        public string BuildingNo { get; set; }
        public string? BuildingName { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public SpaceIdentifier? SpaceIdentifier { get; set; }
        public Agencies? Agencies { get; set; }
        public Streets? Streets { get; set; }
    }
}