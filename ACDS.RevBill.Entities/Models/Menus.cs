using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Menus
	{
        [Key]
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public string? ModuleCode { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        [ForeignKey(nameof(Modules))]
        public int ModuleId { get; set; }
        public Modules? modules { get; set; }
    }
}

