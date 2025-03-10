using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class UpdateArrearsdto
    {
        [Required(ErrorMessage = "Percentage is a required field.")]
        public int Percentage { get; set; }
        [Required(ErrorMessage = "Arrears Applicable is a required field.")]
        public bool ArrearsApplicable { get; set; }
        [Required(ErrorMessage = "Interest Applicable is a required field.")]
        public bool InterestApplicable { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}
