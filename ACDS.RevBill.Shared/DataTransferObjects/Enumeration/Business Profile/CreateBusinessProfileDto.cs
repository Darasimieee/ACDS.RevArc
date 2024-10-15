using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile
{
	public class CreateBusinessProfileDto
	{
        [Required(ErrorMessage = "BusinessTypeId is a required field.")]
        public int BusinessTypeId { get; set; }

        [Required(ErrorMessage = "BusinessSizeId is a required field.")]
        public int BusinessSizeId { get; set; }

        [Required(ErrorMessage = "RevenueId is a required field.")]
        public int RevenueId { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

