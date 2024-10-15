using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
	public class UpdateOrganisationPaymentGatewayDto
	{
        [Required(ErrorMessage = "BankStatus is a required field.")]
        public bool BankStatus { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}