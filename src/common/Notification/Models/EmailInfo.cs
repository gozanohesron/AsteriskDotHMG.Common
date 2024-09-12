namespace AsteriskDotHMG.Notification.Models;

public class EmailInfo: SendGridOptions
{
    public EmailInfo()
    {

    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData, SendGridTrackingOption trackingOption, List<SendGridAttachment> attachments)
    {
        Subject = subject;
        TemplateId = templateId;
        PreHeader = preHeader;
        TemplateData = templateData;
        TrackingOption = trackingOption;
        Attachments = attachments;
    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData, SendGridTrackingOption trackingOption)
        :this(subject, templateId, preHeader, templateData, trackingOption, new())
    {
        
    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData)
        :this(subject, templateId, preHeader, templateData, new())
    {
        
    }

    public string Subject { get; set; }

    public string TemplateId { get; set; }

    public string PreHeader { get; set; }

    public IDictionary<string, dynamic> TemplateData { get; set; }

    public SendGridTrackingOption TrackingOption { get; set; } = new();

    public List<SendGridAttachment> Attachments { get; set; } = new();
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

public class SendGridAttachment: Attachment
{
}