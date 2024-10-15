using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
    public class EnumerationManifestDto
    {
        public int Id { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyAddress { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? PayerID { get; set; }
        public List<BusinessProfileDto>? Businessprofile { get; set; }
        public string? Agency { get; set; }
        public DateTime DateIssued { get; set; }
        public string? OrganisationName { get; set; }
        public byte[]? OrganisationLogo { get; set; }
    }

    public class BusinessProfileDto
    {
        public string? BusinessType { get; set; }
        public string? BusinessSize { get; set; }
        public string? Revenue { get; set; }
    }
}