using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile
{
	public class UpdateBusinessProfileDto
	{
        [Required(ErrorMessage = "BusinessTypeId is a required field.")]
        public int BusinessTypeId { get; set; }

        [Required(ErrorMessage = "BusinessSizeId is a required field.")]
        public int BusinessSizeId { get; set; }

        [Required(ErrorMessage = "RevenueTypeId is a required field.")]
        public int RevenueTypeId { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime? DateModified { get; set; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; set; }
    }
}

