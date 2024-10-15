using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class UserProfiles 
	{
        [Key]
		public int UserProfileId { get; set; }
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        [ForeignKey("Agencies")]
        public int AgencyId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ForeignKey("UserRoles")]
        public int UserRoleId { get; set; }
        public string? Surname { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TenantName { get; set; }
        public bool IsHead { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public Organisation? Organisation { get; set; }
        public UserRoles? UserRoles { get; set; }
    }
}

