using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class Modules
	{
        [Key]
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? ModuleCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool Active { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

