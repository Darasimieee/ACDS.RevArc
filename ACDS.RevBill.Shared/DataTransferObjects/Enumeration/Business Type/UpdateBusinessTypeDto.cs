using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.BusinessType
{
	public class UpdateBusinessTypeDto
	{
        [Required(ErrorMessage = "BusinessTypeName is a required field.")]
        public string? BusinessTypeName { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime? DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}

