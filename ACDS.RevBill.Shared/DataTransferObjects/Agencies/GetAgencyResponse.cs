using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class GetAgencyResponse
    {
        public String? AgencyRef { get; set; }
        public String? FullName { get; set; }
        public String? orcAgency { get; set; }
        public String? HeadRef { get; set; }
        public String? orcAgency2 { get; set; }
    }
}

