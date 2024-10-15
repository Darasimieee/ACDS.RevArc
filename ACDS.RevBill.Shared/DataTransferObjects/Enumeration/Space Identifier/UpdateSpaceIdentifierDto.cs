using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class UpdateSpaceIdentifierDto
	{
        [Required(ErrorMessage = "SpaceIdentifierName is a required field.")]
        public string? SpaceIdentifierName { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime? DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}

