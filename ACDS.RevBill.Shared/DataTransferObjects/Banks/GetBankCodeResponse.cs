using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class GetBankCodeResponse
    {
        public String? BankName { get; set; }
        public String? ContactName { get; set; }
        public String? WebSite { get; set; }
        public String? HOAddress { get; set; }
        public String? CBNCode { get; set; }
        public String? ShortName { get; set; }
        public int CorpID { get; set; }
        public String? Outputdata { get; set; }
    }
}

