using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Customers : EntityBase
    {
        [Key]
        public int CustomerId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ForeignKey("PayerTypes")]
        public int PayerTypeId { get; set; }
        public string? CustomerReferenceNo { get; set; }
        public string? PayerId { get; set; }
        [ForeignKey("Titles")]
        public int TitleId { get; set; }
        public string? CorporateName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? FullName { get; set; }
        [ForeignKey("Genders")]
        public int GenderId { get; set; }
        [ForeignKey("MaritalStatuses")]
        public int MaritalStatusId { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public PayerTypes? PayerTypes { get; set; }
        public Titles? Titles { get; set; }
        public Genders? Genders { get; set; }
        public MaritalStatuses? MaritalStatuses { get; set; }
    }
}