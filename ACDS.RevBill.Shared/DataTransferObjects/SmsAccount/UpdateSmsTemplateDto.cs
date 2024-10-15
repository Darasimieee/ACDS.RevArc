using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Shared.DataTransferObjects.SmsAccount
{
    public record UpdateSmsTemplateDto(string Name, string BccPhoneNumber, string Subject, string Body, bool Active, int SmsAccountId, DateTime DateCreated, string CreatedBy, DateTime DateModified, string ModifiedBy);
 
}
