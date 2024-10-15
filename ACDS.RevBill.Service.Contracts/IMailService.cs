using ACDS.RevBill.Entities.Email;

namespace ACDS.RevBill.Service.Contracts
{
    public interface IMailService
    {
        Task SendWelcomeEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailCustomerAsync(MailRequest mailRequest);
        Task SendForgotPasswordEmailAsync(MailRequest mailRequest);
        Task SendOnboardingOrganisationEmailAsync(MailRequest mailRequest);
        Task SendPasswordUpdateEmailAsync(MailRequest mailRequest);
        Task GenerateAccountActivationEmailAsync(MailRequest mailRequest);
        Task SendApprovedOrganisationOnboardingRequest(MailRequest mailRequest);
        Task SendRejectedOrganisationOnboardingRequest(MailRequest mailRequest);
        Task SendBillGenerationAsync(MailRequest mailRequest);
    }
}