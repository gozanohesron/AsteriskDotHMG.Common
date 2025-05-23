namespace AsteriskDotHMG.Notification.Models;

public class EmailInfo: SendGridOptions
{
    public EmailInfo()
    {

    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData, SendGridTrackingOption trackingOption, List<SendGridAttachment> attachments, IDictionary<string, string> customArgs)
    {
        Subject = subject;
        TemplateId = templateId;
        PreHeader = preHeader;
        TemplateData = templateData;
        TrackingOption = trackingOption;
        Attachments = attachments;
        CustomArgs = customArgs;
    }

    public EmailInfo(string subject, string templateId, string preHeader, IDictionary<string, dynamic> templateData, SendGridTrackingOption trackingOption)
        :this(subject, templateId, preHeader, templateData, trackingOption, new(), new Dictionary<string, string>())
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

    public IDictionary<string, string> CustomArgs { get; set; }
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

public class SendGridResponse
{
    public SendGrid.Response Original { get; }

    public SendGridResponse(SendGrid.Response original)
    {
        StatusCode = original.StatusCode;
        Body = original.Body;
        Headers = original.Headers;
    }

    public HttpStatusCode StatusCode { get; }

    public HttpContent Body { get; }

    public System.Net.Http.Headers.HttpResponseHeaders Headers { get; }
}