using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class CreatePropertyDto
	{
        [Required(ErrorMessage = "AgencyId is a required field.")]
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "SpaceIdentifierId is a required field.")]
        public int SpaceIdentifierId { get; set; }
        [Required(ErrorMessage = "StreetId is a required field.")]
        public int StreetId { get; set; }
        //public int? WardId { get; set; }

        [Required(ErrorMessage = "LocationAddress is a required field.")]
        public string? LocationAddress { get; set; }

        [Required(ErrorMessage = "SpaceFloor is a required field.")]
        public int SpaceFloor { get; set; }

        [Required(ErrorMessage = "BuildingNo is a required field.")]
        public string BuildingNo { get; set; }

        [Required(ErrorMessage = "BuildingName is a required field.")]
        public string? BuildingName { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}