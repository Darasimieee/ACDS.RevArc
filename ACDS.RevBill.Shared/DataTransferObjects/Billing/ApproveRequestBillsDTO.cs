using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class ApproveRequestBillsDTO
    {
        [Required(ErrorMessage = "Status is a required field.")]
        public int ApprovalBillStatusId { get; set; }
        public int BillId { get; set; }
    }
}
