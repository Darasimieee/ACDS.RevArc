using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
    public class CreateOrganisationPaymentGatewayDto
	{
        [Required(ErrorMessage = "BankId is a required field.")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string CreatedBy { get; set; }
    }
}