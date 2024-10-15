using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public class OrganisatationStatus
	{
        [Required(ErrorMessage = "OrganisationId is a required field.")]
        public int OrganisationId { get; set; }
	}
}