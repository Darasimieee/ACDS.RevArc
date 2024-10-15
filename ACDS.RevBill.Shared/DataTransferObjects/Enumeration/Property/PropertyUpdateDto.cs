using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class PropertyUpdateDto
	{
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "SpaceIdentifierId is a required field.")]
        public int SpaceIdentifierId { get; set; }

        [Required(ErrorMessage = "WardId is a required field.")]
        public int WardId { get; set; }

        [Required(ErrorMessage = "LocationAddress is a required field.")]
        public string? LocationAddress { get; set; }

        [Required(ErrorMessage = "SpaceFloor is a required field.")]
        public int SpaceFloor { get; set; }

        [Required(ErrorMessage = "BuildingNo is a required field.")]
        public string BuildingNo { get; set; }

        [Required(ErrorMessage = "BuildingName is a required field.")]
        public string? BuildingName { get; set; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

