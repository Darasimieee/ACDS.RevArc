
using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{ 
    public class PreviewedbillResponse
    {
        public string? StatusMessage { get; set; }
        public object? Data { get; set; }
        public int? itemId { get; set; }
        public int Status { get; set; }
        public CreatePropertyBillUpload Bill { get; set; }

    }
}

