using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class UploadCreationDto
    {
       
        [Required(ErrorMessage = "Street is a required field.")]
        public string? StreetName { get; init; }

        [Required(ErrorMessage = "Description is a required field.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

       
    }
}

