using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency
{
    public class AgencyQuarter4
    {
        public string AgencyName { get; set; }
        public string AgencyRef { get; set; }
        public string OrcAgency { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public decimal Total { get; set; }
        public int TrendYear { get; set; }
        public string StartDate { get; set; }
        public string HeadAgency { get; set; }
        public decimal YtdTotal { get; set; }
    }
}
