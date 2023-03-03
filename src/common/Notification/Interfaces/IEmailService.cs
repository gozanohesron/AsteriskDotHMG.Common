namespace AsteriskDotHMG.Notification.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailInfo emailInfo, string recepient, CancellationToken cancellationToken = default);
    Task<bool> SendEmailAsync(EmailInfo emailInfo, List<string> recepients, CancellationToken cancellationToken = default);
}
