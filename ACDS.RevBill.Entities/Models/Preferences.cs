using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class Preferences
    {
        [Key] public int PreferenceId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [StringLength(100)] public string? Key { get; set; }
        [StringLength(512)] public string? Value { get; set; } 
        public DateTime? DateCreated { get; set; }
        [StringLength(300)] public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        [StringLength(300)] public string? ModifiedBy { get; set; }
    }
}