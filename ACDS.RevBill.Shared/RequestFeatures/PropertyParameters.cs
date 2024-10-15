using System;
namespace ACDS.RevBill.Shared.RequestFeatures
{
	public class PropertyParameters : RequestParameters
    {
		
		public string? SpaceIdentifier { get; set; }
		public string? AreaOffice { get; set; }
        public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; } = DateTime.MaxValue;

		public bool ValidDateRange => EndDate.Date >= StartDate.Date;

		public string? Building { get; set; }
        public string? Address { get; set; }
    }
}