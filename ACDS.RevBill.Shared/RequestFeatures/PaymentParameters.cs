namespace ACDS.RevBill.Shared.RequestFeatures
{
	public class PaymentParameters : RequestParameters
    {
        public string? PayerId { get; set; }
        public string? Agency { get; set; }
        public string? Revenue { get; set; }
        public string? BankCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;

        public bool ValidDateRange => EndDate.Date >= StartDate.Date;
    }
}