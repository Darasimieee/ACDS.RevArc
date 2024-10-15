using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.SmsAccount
{
    public record GetSmsTemplateDto(int SmsTemplateId, string Name,string BccPhoneNumber,string Subject, string Body,bool Active,int SmsAccountId);
 
}
