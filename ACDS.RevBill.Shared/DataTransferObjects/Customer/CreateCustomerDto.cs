using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Customer
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "PayerTypeId is a required field.")]
        public int PayerTypeId { get; set; }

        [Required(ErrorMessage = "PayerId is a required field.")]
        public string? PayerId { get; set; }

        [Required(ErrorMessage = "TitleId is a required field.")]
        public int TitleId { get; set; }

        public string? CorporateName { get; set; }

        [Required(ErrorMessage = "FirstName is a required field.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is a required field.")]
        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "GenderId is a required field.")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "MaritalStatusId is a required field.")]
        public int MaritalStatusId { get; set; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "PhoneNo is a required field.")]
        [MaxLength(11, ErrorMessage = "Requird length for the phone number is 11 characters.")]
        public string? PhoneNo { get; set; }

        [Required(ErrorMessage = "EnumerationStatus is a required field.")] //0 if they're not willing to supply their pid, 1 if they are willing 
        public bool SuppliedPID { get; set; }

        [Required(ErrorMessage = "DateCreated is a required field.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}