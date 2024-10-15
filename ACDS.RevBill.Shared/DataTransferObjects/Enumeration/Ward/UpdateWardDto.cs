using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class UpdateWardDto
	{
        [Required(ErrorMessage = "WardName is a required field.")]
        public string? WardName { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime? DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}

