namespace ACDS.RevBill.Shared.DataTransferObjects.Payment
{
    public class PaymentbyRevenueDto
    {
        public int Id { get; set; }
        public int BillCount { get; set; }
        public string? BillValue { get; set; }
        public string? BillPaid { get; set; }
        public string? BillOutstanding { get; set; }

        public string? RevenueName { get; set; }
    }
}