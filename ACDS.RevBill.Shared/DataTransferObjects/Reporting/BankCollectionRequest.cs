namespace ACDS.RevBill.Shared.DataTransferObjects.Reporting
{
	public class BankCollectionRequest
	{
		public int uCode { get; set; }
        public string ipAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartEntryID { get; set; }
        public string EndEntryID { get; set; }
        public string AcctType { get; set; }
        public string Bank { get; set; }
        public string RevCode { get; set; }
        public string AgencyRef { get; set; }
        public string Owner { get; set; }
        public string BankAcct { get; set; }
    }
}