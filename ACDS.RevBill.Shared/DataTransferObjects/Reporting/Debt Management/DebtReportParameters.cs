using ACDS.RevBill.Shared.RequestFeatures;
using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class DebtReportParameters : BillingParameters
    {
        public string? PayerTypeId { get; set; }
        public string? Revenue { get; set; }
        public int? stateId { get; set; }
        public int? LcdaId { get; set; }
        public int? LgaId{ get; set; }
            
    }
 }

