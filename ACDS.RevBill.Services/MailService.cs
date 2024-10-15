using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Email;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Service.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NLog;

namespace ACDS.RevBill.Services
{
    public class MailService : IMailService
    {
        private DataContext _context;
        private readonly EmailConfiguration _emailConfig;
        private readonly ILoggerManager _logger;
        private int Port { get; set; }
        private string? SmtpServer { get; set; }
        private string? Username { get; set; }
        private string? Password { get; set; }

        public MailService(DataContext context, EmailConfiguration emailConfig, ILoggerManager logger)
        {
            _context = context;
            _emailConfig = emailConfig;
            _logger = logger;
        }

        public async Task SendOnboardingOrganisationEmailAsync(MailRequest mailRequest)
        {
            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//organisationOnboarding.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[organisation]", mailRequest.OrganisationName);

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_emailConfig.UserName);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }           
        }

        public async Task SendApprovedOrganisationOnboardingRequest(MailRequest mailRequest)
        {
            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//onboardingApproval.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
               .Replace("[organisation]", mailRequest.OrganisationName)
               .Replace("[email]", mailRequest.ToEmail)
               .Replace("[password]", mailRequest.Password)
               .Replace("[otp]", mailRequest.OTP);

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_emailConfig.UserName);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
        }

        public Task SendRejectedOrganisationOnboardingRequest(MailRequest mailRequest)
        {
            throw new NotImplementedException();
        }

        public async Task SendWelcomeEmailAsync(MailRequest mailRequest)
        {
            var emailConfig = _context.EmailAccounts.Select(x => x).ToList();
            foreach (var x in emailConfig)
            {
                SmtpServer = x.SmtpServer;
                Port = x.Port;
                Username = x.Username;
                Password = x.Password;
            }

            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//emailVerify.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
                .Replace("[email]", mailRequest.ToEmail)
                .Replace("[password]", mailRequest.Password)
                .Replace("[otp]", mailRequest.OTP)
                .Replace("[first_name]", mailRequest.FirstName)
                .Replace("[last_name]", mailRequest.LastName);

            try
            {
                _logger.LogInfo("I got here");
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(Username);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(SmtpServer, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Username, Password);
                
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
            _logger.LogInfo("Mail sent" + mailRequest.Body);
        }

        public async Task SendWelcomeEmailCustomerAsync(MailRequest mailRequest)
        {
            var emailConfig = _context.EmailAccounts.Select(x => x).ToList();
            foreach (var x in emailConfig)
            {
                SmtpServer = x.SmtpServer;
                Port = x.Port;
                Username = x.Username;
                Password = x.Password;
            }

            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//emailVerifyCustomer.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
                .Replace("[email]", mailRequest.ToEmail)
                .Replace("[otp]", mailRequest.OTP)
                .Replace("[first_name]", mailRequest.FirstName)
                .Replace("[last_name]", mailRequest.LastName);

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(Username);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(SmtpServer, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Username, Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
        }

        public async Task SendForgotPasswordEmailAsync(MailRequest mailRequest)
        {
            var emailConfig = _context.EmailAccounts.Select(x => x).ToList();
            foreach (var x in emailConfig)
            {
                SmtpServer = x.SmtpServer;
                Port = x.Port;
                Username = x.Username;
                Password = x.Password;
            }

            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//passwordForgot.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
                .Replace("[email]", mailRequest.ToEmail)
                .Replace("[otp]", mailRequest.OTP);

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(Username);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(SmtpServer, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Username, Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
        }

        public async Task SendPasswordUpdateEmailAsync(MailRequest mailRequest)
        {
            var emailConfig = _context.EmailAccounts.Select(x => x).ToList();
            foreach (var x in emailConfig)
            {
                SmtpServer = x.SmtpServer;
                Port = x.Port;
                Username = x.Username;
                Password = x.Password;
            }

            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//PasswordUpdate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
                .Replace("[email]", mailRequest.ToEmail);

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(Username);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(SmtpServer, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Username, Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
        }

        public async Task GenerateAccountActivationEmailAsync(MailRequest mailRequest)
        {
            var emailConfig = _context.EmailAccounts.Select(x => x).ToList();
            foreach (var x in emailConfig)
            {
                SmtpServer = x.SmtpServer;
                Port = x.Port;
                Username = x.Username;
                Password = x.Password;
            }

            string FilePath = Directory.GetCurrentDirectory() + "//Templates//emails//accountActivation.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
                .Replace("[email]", mailRequest.ToEmail)
                .Replace("[otp]", mailRequest.OTP);

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(Username);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(SmtpServer, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Username, Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
        }

        public async Task SendBillGenerationAsync(MailRequest mailRequest)
        {
            var emailConfig = _context.EmailAccounts.Select(x => x).ToList();
            foreach (var x in emailConfig)
            {
                SmtpServer = x.SmtpServer;
                Port = x.Port;
                Username = x.Username;
                Password = x.Password;
            }

            string FilePath = Directory.GetCurrentDirectory() + "//Templates//billing//generated-bill.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText
                .Replace("[details]", mailRequest.Body)
                .Replace("[first_name]", mailRequest.FirstName)
                .Replace(" [last_name]", mailRequest.LastName);
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(Username);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(SmtpServer, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Username, Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                Console.WriteLine($"Error:{ex}");
            }
            _logger.LogInfo("I got here" + mailRequest.Body);
        }

    }
}