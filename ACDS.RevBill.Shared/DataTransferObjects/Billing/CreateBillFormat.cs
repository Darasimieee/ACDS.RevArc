using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreateBillFormat
	{
        [Required(ErrorMessage = "SignatureOne is a required field.")]
        public IFormFile SignatureOne { get; set; }

        public IFormFile? SignatureTwo { get; set; }

        [Required(ErrorMessage = "ContentOne is a required field.")]
        public string ContentOne { get; set; }

        [Required(ErrorMessage = "ContentTwo is a required field.")]
        public string ContentTwo { get; set; }

        [Required(ErrorMessage = "ClosingSection is a required field.")]
        public string ClosingSection { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}