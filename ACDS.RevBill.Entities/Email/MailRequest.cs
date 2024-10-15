using System;
using Microsoft.AspNetCore.Http;

namespace ACDS.RevBill.Entities.Email
{
    public class MailRequest
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? OrganisationName { get; set; }
        public string? Password { get; set; }
        public string? OTP { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}

