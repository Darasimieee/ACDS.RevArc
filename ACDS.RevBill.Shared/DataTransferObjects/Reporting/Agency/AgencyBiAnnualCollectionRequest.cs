using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency
{
    public class AgencyBiAnnualCollectionRequest
    {
        public int TrendYear { get; set; }
        public int BiAnnual { get; set; }
        public string AgencyRef { get; set; }
    }
}
