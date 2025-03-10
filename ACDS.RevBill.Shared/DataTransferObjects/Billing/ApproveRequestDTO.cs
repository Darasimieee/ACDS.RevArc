using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class ApproveRequestDTO
    {
        public List<ApproveRequestBillsDTO> BillIds { get; set; }
        public DateTime? DateModified { get; set; }
        [Required(ErrorMessage = "Editor is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}
