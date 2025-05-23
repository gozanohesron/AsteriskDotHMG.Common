namespace AsteriskDotHMG.Notification.Interfaces;

public interface IEmailService
{
    Task<SendGridResponse> SendEmailAsync(EmailInfo emailInfo, string recepient, CancellationToken cancellationToken = default);
    Task<SendGridResponse> SendEmailAsync(EmailInfo emailInfo, List<string> recepients, CancellationToken cancellationToken = default);
}
