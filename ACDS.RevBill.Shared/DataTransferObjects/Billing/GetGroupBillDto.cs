using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
    public class GetGroupBillDto
    {
        public string? RevenueName { get; set; }
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
