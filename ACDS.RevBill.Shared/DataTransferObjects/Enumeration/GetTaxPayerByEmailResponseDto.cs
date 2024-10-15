using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class GetTaxPayerByEmailResponseDto
	{
        public string? PayerID { get; set; }
        public string? Title { get; set; }
        public string? FullName { get; set; }
        public string? SurName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? GSM { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
    }
}

