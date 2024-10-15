using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class ModuleComponents
    {
        [Key]public int ModuleComponentId { get; set; }
        [StringLength(5)] public string? ModuleCode { get; set; }
        [StringLength(5)] public string? ComponentCode { get; set; }
        public int Order { get; set; }
        public DateTime? DateCreated { get; set; }
        [StringLength(300)] public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        [StringLength(300)] public string? ModifiedBy { get; set; }
    }
}

