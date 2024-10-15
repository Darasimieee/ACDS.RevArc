using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACDS.RevBill.Services
{
    public class PayerIdExtractor
    {
        public PayerIdResult GetPayerId(string ebsResult)
        {
            string pattern = @"(C|N)-\d+";

            Regex rg = new Regex(pattern);
            var test = rg.Match(ebsResult);

            if (test.Success)
            {
                return new PayerIdResult
                {
                    Successful = true,
                    PayerId = test.Value
                };
            }

            return new PayerIdResult
            {
                Successful = false,
                PayerId = ""
            };
        }
    }

    public class PayerIdResult
    {
        public string PayerId { get; set; }

        public bool Successful { get; set; }
    }
}
