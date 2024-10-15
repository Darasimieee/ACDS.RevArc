using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record CreateEmailAccountDto(string Email, string DisplayName, string Host, string SmtpServer, int Port, string Username, string Password, bool EnableSSL,bool EmailAccountStatus, bool UseDefaultCredentials,
        DateTime DateCreated, string CreatedBy, int OrganisationId);
}

