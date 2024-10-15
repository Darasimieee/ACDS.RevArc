using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetEmailAccountDto(int EmailAccountId, string Email, string DisplayName, string Host, string SmtpServer, int Port, string Username, string Password, bool EnableSSL, bool EmailAccountStatus, bool UseDefaultCredentials,
        DateTime DateCreated, string CreatedBy, DateTime DateModified, string ModifiedBy, int OrganisationId);
}

