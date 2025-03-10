using System;
using System.ComponentModel.DataAnnotations;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class CreateArrearsDTO
    {


        [Required(ErrorMessage = "Year is a required field.")]
        public string? Year { get; set; }
        [Required(ErrorMessage = "Percentage is a required field.")]
        public int Percentage { get; set; }
        [Required(ErrorMessage = "Arrears Applicable is a required field.")]
        public bool ArrearsApplicable { get; set; }
        [Required(ErrorMessage = "Interest Applicable is a required field.")]
        public bool InterestApplicable { get; set; }
        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

