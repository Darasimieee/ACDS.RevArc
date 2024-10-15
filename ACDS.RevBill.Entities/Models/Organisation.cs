using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
    public class Organisation
    {
        [Key]
        public int OrganisationId { get; set; }
        public int CountryId { get; set; }
        public string? PayerId { get; set; }
        public string? OrganisationName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int StateId { get; set; }
        public int LgaId { get; set; }
        public int LcdaId { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? WebUrl { get; set; }
        public byte[]? LogoData { get; set; }
        public string? LogoName { get; set; }
        public byte[]? BackgroundImagesData { get; set; }
        public string? BackgroundImagesName { get; set; }
        public string? BillSchema { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        public string? AgencyCode { get; set; }
        public string? RevenueCode { get; set; }
        public bool? OrganisationStatus { get; set; }
        public int OrganisationApprovalStatus { get; set; }
        public string? TenantName { get; set; }
    }
}
