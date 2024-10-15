using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetEmailTemplateDto(int EmailTemplateId, string Name, string BccEmailAddress, string ToEmailAddress, string ToEmailGroupName, string Subject, string Body, bool Active, DateTime DateCreated, string CreatedBy, DateTime DateModified, string ModifiedBy, int EmailAccountId);

}

