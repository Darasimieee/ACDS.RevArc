namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
    public class PaymentbyBankDto
    {
        public int Id { get; set; }
        public int BillCount { get; set; }
        public string?  BillValue { get; set; }
        public string? BillPaid { get; set; }
        public string? BankName { get; set; }
       
       
    }
}