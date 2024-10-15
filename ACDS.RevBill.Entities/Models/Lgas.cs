using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class Lgas
    {
        [Key]
        public long Id { get; set; }
        public string? LGAName { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string? CreatedBy { get; set; }
        public int RecordStatus { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
