using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class UserMenus : EntityBase
    {
		[Key]
		public int UserMenuId { get; set; }
        [ForeignKey(nameof(Menus))]
        public int MenuId { get; set; }
        [ForeignKey(nameof(Roles))]
        public int RoleId { get; set; }
        public long UserId { get; set; }
        [ForeignKey("OrganisationModule")]
        public int OrganisationModuleId { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

