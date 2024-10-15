using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public class CreateTenancyDto
	{
        [Required(ErrorMessage = "TenantId is a required field.")]
        public string? TenantId { get; set; }

        [Required(ErrorMessage = "ConnectionString is a required field.")]
        public string? ConnectionString { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}