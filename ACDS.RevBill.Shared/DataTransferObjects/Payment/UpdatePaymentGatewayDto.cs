using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
	public class UpdatePaymentGatewayDto
    {
        [Required(ErrorMessage = "BankName is a required field.")]
        public string BankName { get; init; }

        [Required(ErrorMessage = "BankUrl is a required field.")]
        public string BankUrl { get; init; }

        [Required(ErrorMessage = "BankDescription is a required field.")]
        public string BankDescription { get; init; }

        public IFormFile? BankLogo { get; set; }

        [Required(ErrorMessage = "BankStatus is a required field.")]
        public bool BankStatus { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}