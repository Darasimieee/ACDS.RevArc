using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize
{
	public class CreateBusinessSizeDto
	{
        [Required(ErrorMessage = "BusinessSizeName is a required field.")]
        public string? BusinessSizeName { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

