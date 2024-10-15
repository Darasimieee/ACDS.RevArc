using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
    public class CreatePaymentDto
	{
        [Required(ErrorMessage = "PayerId is a required field.")]
        public int PayerId { get; set; }

        [Required(ErrorMessage = "EntryId is a required field.")]
        public int EntryId { get; set; }

        [Required(ErrorMessage = "WebGuid is a required field.")]
        public string WebGuid { get; set; }

        [Required(ErrorMessage = "AssessRef is a required field.")]
        public string AssessRef { get; set; }

        [Required(ErrorMessage = "EntryDate is a required field.")]
        public DateTime EntryDate { get; set; }

        [Required(ErrorMessage = "PayerType is a required field.")]
        public string PayerType { get; set; }

        [Required(ErrorMessage = "Agency is a required field.")]
        public string Agency { get; set; }

        [Required(ErrorMessage = "Revenue is a required field.")]
        public string Revenue { get; set; }

        [Required(ErrorMessage = "Amount is a required field.")]
        public decimal Amount { get; init; }

        [Required(ErrorMessage = "BankAmount is a required field.")]
        public decimal BankAmount { get; init; }

        [Required(ErrorMessage = "BankEntryDate is a required field.")]
        public DateTime BankEntryDate { get; set; }

        [Required(ErrorMessage = "BankTransId is a required field.")]
        public int BankTransId { get; set; }
        [Required(ErrorMessage = "BankCode is a required field.")]
        public string BankCode { get; set; }

        [Required(ErrorMessage = "BankTranRef is a required field.")]
        public string BankTranRef { get; set; }
        public string? Bankbf { get; set; }
    }
}