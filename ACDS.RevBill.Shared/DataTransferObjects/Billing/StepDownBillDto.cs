using System;
using System.ComponentModel.DataAnnotations;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class StepDownBillDto
	{
        [Required(ErrorMessage = "Status is a required field.")]
        public int ApprovalBillStatusId { get; set; }
        [Required(ErrorMessage = "Approver is a required field.")]
        public string? Approver { get; set; }
        public DateTime? DateModified { get; set; }
        [Required(ErrorMessage = "Editor is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}