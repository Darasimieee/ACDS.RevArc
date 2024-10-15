using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Customer
{
	public class UpdateCustomerDto
	{
        [Required(ErrorMessage = "OrganisationId is a required field.")]
        public int OrganisationId { get; init; }

        [Required(ErrorMessage = "PayerTypeId is a required field.")]
        public int PayerTypeId { get; init; }

        [Required(ErrorMessage = "PayerId is a required field.")]
        public string? PayerId { get; init; }

        [Required(ErrorMessage = "TitleId is a required field.")]
        public int TitleId { get; init; }

        public string? CorporateName { get; init; }

        [Required(ErrorMessage = "FirstName is a required field.")]
        public string? FirstName { get; init; }

        [Required(ErrorMessage = "LastName is a required field.")]
        public string? LastName { get; init; }

        public string? MiddleName { get; init; }

        [Required(ErrorMessage = "GenderId is a required field.")]
        public int GenderId { get; init; }

        [Required(ErrorMessage = "MaritalStatusId is a required field.")]
        public int MaritalStatusId { get; init; }

        [Required(ErrorMessage = "AddressNo is a required field.")]
        public string? AddressNo { get; init; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string? Address { get; init; }

        [Required(ErrorMessage = "Email is a required field.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; init; }

        [Required(ErrorMessage = "PhoneNo is a required field.")]
        [MaxLength(11, ErrorMessage = "Maximum length for the Phone Number is 11 characters.")]
        public string? PhoneNo { get; init; }

        [Required(ErrorMessage = "CustomerStatus is a required field.")]
        public bool CustomerStatus { get; init; }

        [Required(ErrorMessage = "Date Modified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

