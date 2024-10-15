using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.SmsAccount
{
    public record GetSmsAccountDto(int SmsAccountId, string SmsGateway,string DisplayName, string SendSmsEndPoint, string Username,string Password,string BalanceEnqEndPoint,bool Active);
 
}
