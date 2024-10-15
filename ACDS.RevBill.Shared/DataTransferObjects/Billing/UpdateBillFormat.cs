using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class UpdateBillFormat
	{
       
        public IFormFile SignatureOne { get; set; }

        public IFormFile? SignatureTwo { get; set; }

        [Required(ErrorMessage = "ContentOne is a required field.")]
        public string ContentOne { get; set; }

        [Required(ErrorMessage = "ContentTwo is a required field.")]
        public string ContentTwo { get; set; }

        [Required(ErrorMessage = "ClosingSection is a required field.")]
        public string ClosingSection { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}