using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency
{
    public class AgencyBiAnnualCollectionResponse
    {
        public string AgencyName { get; set; }
        public string AgencyRef { get; set; }
        public string OrcAgency { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Total { get; set; }
        public int TrendYear { get; set; }
        public string StartDate { get; set; }
        public string HeadAgency { get; set; }
        public decimal YtdTotal { get; set; }
    }
}
