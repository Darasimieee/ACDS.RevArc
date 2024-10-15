using System;
namespace ACDS.RevBill.Shared.RequestFeatures
{
	public class BillingParameters : RequestParameters
	{
        public string? PayerId { get; set; }
        public string? CustomerName { get; set; }
        public string? AreaOffice { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;

        public bool ValidDateRange => EndDate.Date >= StartDate.Date;
    }
}

