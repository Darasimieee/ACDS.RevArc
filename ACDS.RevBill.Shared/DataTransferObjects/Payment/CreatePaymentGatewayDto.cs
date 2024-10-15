using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
	public class CreatePaymentGatewayDto
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

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string CreatedBy { get; init; }
    }
}