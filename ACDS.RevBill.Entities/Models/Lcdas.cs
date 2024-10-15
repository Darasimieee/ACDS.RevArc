using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class Lcdas
    {
        [Key]
        public long Id { get; set; }
        public string? LCDAName { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? TextLocation { get; set; }
        public int? EbsLgaId { get; set; }
        public int? EbsLcdaId { get; set; }
        public int? LGAId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string? CreatedBy { get; set; }
        public int RecordStatus { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
