using System;
namespace ACDS.RevBill.Entities.Email
{
	public class EmailConfiguration
	{
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}

