using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class PreferenceModes
    {
        [Key] public int PreferenceId { get; set; }
        [StringLength(30)] public string? Title { get; set; }
        [StringLength(30)] public string? Caption { get; set; }
        [StringLength(300)] public string? Question { get; set; }
        [StringLength(2000)] public string? Narration { get; set; }
        public int State { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
