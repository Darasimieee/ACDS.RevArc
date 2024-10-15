using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class VerifyPidResponseDto
	{
		public string? PayerID { get; set; }
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? GSM { get; set; }
        public string? BranchName { get; set; }
        public string? SurName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Title { get; set; }
        public string? birthdate { get; set; }
        public string? Sex { get; set; }
        public string? maritalstatus { get; set; }
        public string? BVN_Exist { get; set; }
        public string? CorpID { get; set; }
        public string? CorporateName { get; set; }
        public string? NIN_Exist { get; set; }
    }
}

