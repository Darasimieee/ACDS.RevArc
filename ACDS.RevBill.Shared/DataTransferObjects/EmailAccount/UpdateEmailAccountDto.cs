using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record UpdateEmailAccountDto(string Email, string DisplayName, string Host, string SmtpServer, int Port, string Username, string Password, bool EnableSSL, bool EmailAccountStatus, bool UseDefaultCredentials,
        DateTime DateModified, string ModifiedBy, int OrganisationId);
}

