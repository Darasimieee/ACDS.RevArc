using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessSize
{
	public class UpdateBusinessSizeDto
	{
        [Required(ErrorMessage = "BusinessSizeName is a required field.")]
        public string? BusinessSizeName { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime? DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}

