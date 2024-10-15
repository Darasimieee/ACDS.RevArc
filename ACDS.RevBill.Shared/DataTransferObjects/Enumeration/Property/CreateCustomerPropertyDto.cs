using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Property
{
	public class CreateCustomerPropertyDto
	{
        [Required(ErrorMessage = "DoesCustomerExist is a required field.")]
        public bool DoesCustomerExist { get; set; }

        [Required(ErrorMessage = "CustomerId is a required field.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}