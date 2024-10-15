using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.SmsAccount
{
    public record UpdateSmsAccountDto(string SmsGateway, string DisplayName, string SendSmsEndPoint, string Username, string Password, string BalanceEnqEndPoint, bool Active, DateTime DateCreated, string CreatedBy, DateTime DateModified, string ModifiedBy);
 
}
