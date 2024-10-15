using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class UpdateOrganisationDto
    {
        [Required(ErrorMessage = "CountryId is a required field."), Range(0, 195)]
        public int CountryId { get; init; }

        [Required(ErrorMessage = "Organisation Name is a required field.")]
        public string? OrganisationName { get; init; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string? Address { get; init; }

        [Required(ErrorMessage = "City is a required field.")]
        public string? City { get; init; }

        [Required(ErrorMessage = "StateId is a required field."), Range(0, 37)]
        public int StateId { get; init; }

        [Required(ErrorMessage = "LgaId is a required field."), Range(0, 1000)]
        public int LgaId { get; init; }

        //[Required(ErrorMessage = "LcdaId is a required field."), Range(0, 36)]
        public int LcdaId { get; init; }

        [Required(ErrorMessage = "PhoneNo is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string? PhoneNo { get; init; }

        [Required(ErrorMessage = "Email is a required field.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; init; }

        //[Required(ErrorMessage = "WebUrl is a required field.")]
        public string? WebUrl { get; init; }

        public IFormFile? Logo { get; set; }

        public IFormFile? BackgroundImage { get; set; }

        //[Required(ErrorMessage = "BillSchema is a required field.")]
        public string? BillSchema { get; init; }

        [Required(ErrorMessage = "OrganisationStatus is a required field.")]
        public bool OrganisationStatus { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}