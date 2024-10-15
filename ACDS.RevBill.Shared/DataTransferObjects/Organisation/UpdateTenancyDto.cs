using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public class UpdateTenancyDto
	{
        [Required(ErrorMessage = "DataSource is a required field.")]
        public string? DataSource { get; set; }

        [Required(ErrorMessage = "UserID is a required field.")]
        public string? UserID { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "InitialCatalog is a required field.")]
        public string? InitialCatalog { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}