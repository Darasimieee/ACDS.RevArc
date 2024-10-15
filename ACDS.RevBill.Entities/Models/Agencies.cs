using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class Agencies : EntityBase
    {
        [Key]
        public int AgencyId { get; set; }
        public int OrganisationId { get; set; }
        public string? AgencyCode { get; set; }
        public string? AgencyName { get; set; }
        public string? Description { get; set; }
        public bool IsHead { get; set; }
        public bool Active { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
    }
}