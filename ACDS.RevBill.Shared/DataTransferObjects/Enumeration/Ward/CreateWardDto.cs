using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class CreateWardDto
	{
        [Required(ErrorMessage = "WardName is a required field.")]
        public string? WardName { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

