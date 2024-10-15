using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class GetRevenueResponse
    {
        public String? RevCode { get; set; }
        public String? AgencyHead { get; set; }
        public String? RevName { get; set; }
        public String? orcRev { get; set; }
        public String? AcctNameID { get; set; }
        public String? Rank  { get; set; }
    }
}

