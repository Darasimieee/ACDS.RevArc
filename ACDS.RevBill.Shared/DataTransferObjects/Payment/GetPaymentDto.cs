namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
    public class GetPaymentDto
    {
        public long PaymentId { get; set; }
        public int PayerId { get; set; }
        public int EntryId { get; set; }
        public string WebGuid { get; set; }
        public string AssessRef { get; set; }
        public DateTime EntryDate { get; set; }
        public string PayerType { get; set; }
        public string Agency { get; set; }
        public string Revenue { get; set; }
        public int BankCode { get; set; }
        public decimal Amount { get; set; }
        public decimal BankAmount { get; set; }
        public DateTime BankEntryDate { get; set; }
        public int BankTransId { get; set; }
        public string BankTranRef { get; set; }
    }
}