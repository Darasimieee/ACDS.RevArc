using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class RoleModuleMenus : EntityBase
    {
		[Key]
		public int RoleModuleMenusId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        [ForeignKey("Modules")]
        public int ModuleId { get; set; }

        [ForeignKey("Menus")]
        public int MenuId { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        public Menus? Menus { get; set; }
        public Modules? Modules { get; set; }
        public Roles? Roles { get; set; }
    }
}