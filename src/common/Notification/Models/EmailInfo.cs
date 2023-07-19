namespace AsteriskDotHMG.Notification.Models;

public class EmailInfo
{
    public EmailInfo()
    {

    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData, SendGridTrackingOption trackingOption)
    {
        Subject = subject;
        TemplateId = templateId;
        PreHeader = preHeader;
        TemplateData = templateData;
        TrackingOption = trackingOption;
    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData)
    {
        Subject = subject;
        TemplateId = templateId;
        PreHeader = preHeader;
        TemplateData = templateData;
        TrackingOption = new();
    }

    public string Subject { get; set; }

    public string TemplateId { get; set; }

    public string PreHeader { get; set; }

    public IDictionary<string, dynamic> TemplateData { get; set; }

    public SendGridTrackingOption TrackingOption { get; set; } = new();

    public string ApiKey { get; set; }
}

public class SendGridTrackingOption
{
    public SendGridTrackingOption()
    {

    }

    public SendGridTrackingOption(bool allowClick, bool allowOpen, bool allowGoogleAnalytics, bool allowSubscription)
    {
        AllowClick = allowClick;
        AllowOpen = allowOpen;
        AllowGoogleAnalytics = allowGoogleAnalytics;
        AllowSubscription = allowSubscription;
    }

    public bool AllowClick { get; set; }

    public bool AllowOpen { get; set; }

    public bool AllowGoogleAnalytics { get; set; }

    public bool AllowSubscription { get; set; }
}
