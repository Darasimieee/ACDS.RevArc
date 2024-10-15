using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class CreateSpaceIdentifierDto
	{
        [Required(ErrorMessage = "SpaceIdentifierName is a required field.")]
        public string? SpaceIdentifierName { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

