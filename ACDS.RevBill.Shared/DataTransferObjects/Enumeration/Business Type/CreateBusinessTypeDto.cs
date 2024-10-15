using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType
{
	public class CreateBusinessTypeDto
	{
        [Required(ErrorMessage = "BusinessTypeName is a required field.")]
        public string? BusinessTypeName { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

