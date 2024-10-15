using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class UserRoles
    {
		[Key]
		public int UserRoleId { get; set; }
        [ForeignKey(nameof(Roles))]
        public int RoleId { get; set; }        
        public int UserId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public string? TenantName { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public Roles? Roles { get; set; }
    }
}

