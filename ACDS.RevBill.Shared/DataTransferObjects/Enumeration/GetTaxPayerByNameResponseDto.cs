using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class GetTaxPayerByNameResponseDto
	{
		public string? PersID_abc { get; set; }
        public string? Title { get; set; }
        public string? FullName { get; set; }
        public string? SurName { get; set; }
        public string? Exact2 { get; set; }
        public string? Email { get; set; }
        public string? GSM { get; set; }
        public string? Address { get; set; }
        public string? CorporateID { get; set; }
        public string? BVN { get; set; }
        public string? NIN { get; set; }
    }
}