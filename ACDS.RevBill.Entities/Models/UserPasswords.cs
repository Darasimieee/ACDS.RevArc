using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class UserPasswords 
    {
        [Key]
        public int UserPasswordId { get; set; }
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public string? Password { get; set; }
        public string? TenantName { get; set; }
        public DateTime? DateCreated { get; set; }
        [StringLength(300)] public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        [StringLength(300)] public string? ModifiedBy { get; set; }
    }
}
