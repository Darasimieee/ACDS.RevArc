using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class Arrears:EntityBase
    {
        [Key]
        public int ArrearId { get; set; }

        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public string? Year { get; set; }
        public int Percentage { get; set; }
        public bool ArrearsApplicable { get; set; }
        public bool InterestApplicable { get; set; }
        public bool Active { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        public Organisation? organisation { get; set; }
    }
}
