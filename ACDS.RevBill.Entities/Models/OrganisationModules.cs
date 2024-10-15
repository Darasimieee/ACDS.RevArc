using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class OrganisationModules
    {
		[Key]
		public int OrganisationModuleId { get; set; }       
        [ForeignKey("Modules")]
        public int ModuleId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }        
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public Modules? Modules { get; set; }
    }
}

