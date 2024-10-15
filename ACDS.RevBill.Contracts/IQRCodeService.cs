using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Contracts
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string data);
    }
}
