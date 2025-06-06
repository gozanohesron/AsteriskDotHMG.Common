﻿namespace AsteriskDotHMG.Notification.Services;

public class SendGridEmailService : IEmailService
{

    private readonly SendGridOptions _options;

    public SendGridEmailService(IOptions<SendGridOptions> options)
    {
        _options = options.Value;
    }

    public Task<SendGridResponse> SendEmailAsync(EmailInfo emailInfo, string recepient, CancellationToken cancellationToken = default)
    {
        return SendEmailAsync(emailInfo, new List<string> { recepient }, cancellationToken);
    }

    public async Task<SendGridResponse> SendEmailAsync(EmailInfo emailInfo, List<string> recepients, CancellationToken cancellationToken = default)
    {
        try
        {
            string key = _options.ApiKey;

            if (!string.IsNullOrEmpty(emailInfo.ApiKey))
            {
                key = emailInfo.ApiKey;
            }

            SendGridClient client = new(key);

            string subject = emailInfo.Subject;
            EmailAddress from = new(_options.SenderEmail, _options.SenderName);

            if (!string.IsNullOrEmpty(emailInfo.SenderName) && !string.IsNullOrEmpty(emailInfo.SenderEmail))
            {
                from = new(emailInfo.SenderEmail, emailInfo.SenderName);
            }

            List<EmailAddress> tos = recepients.Select(e => new EmailAddress(e)).ToList();

            SendGridMessage msg = new();

            if (!string.IsNullOrEmpty(emailInfo.TemplateId))
            {
                msg = MailHelper.CreateSingleTemplateEmailToMultipleRecipients(from, tos, emailInfo.TemplateId, emailInfo.TemplateData);
            }
            else
            {
                string stringContent = emailInfo.TemplateData.Where(e => e.Key == Constants.SENDGRID_STRING_CONTENT_VARIABLE).FirstOrDefault().Value;
                string htmlContent = emailInfo.TemplateData.Where(e => e.Key == Constants.SENDGRID_HTML_CONTENT_VARIABLE).FirstOrDefault().Value;

                msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, emailInfo.Subject, stringContent, htmlContent);
            }

            if (emailInfo.Attachments != null && emailInfo.Attachments.Count > 0)
            {
                msg.AddAttachments(emailInfo.Attachments);
            }

            SendGridTrackingOption trackingOption = emailInfo.TrackingOption ?? new();

            msg.SetClickTracking(trackingOption.AllowClick, trackingOption.AllowClick);
            msg.SetOpenTracking(trackingOption.AllowOpen);
            msg.SetGoogleAnalytics(trackingOption.AllowGoogleAnalytics);
            msg.SetSubscriptionTracking(trackingOption.AllowSubscription);

            if (emailInfo.CustomArgs != null && emailInfo.CustomArgs.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in emailInfo.CustomArgs)
                {
                    msg.AddCustomArg(pair.Key, pair.Value);
                }
            }

            SendGrid.Response response = await client.SendEmailAsync(msg, cancellationToken);
            return new(response);
            
        }
        catch (Exception)
        {
            throw;
        }
    }
}
