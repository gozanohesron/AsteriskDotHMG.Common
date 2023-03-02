namespace AsteriskDotHMG.Notification.Models;

public class EmailInfo
{
    public string Subject { get; set; }

    public string TemplateId { get; set; }

    public string PreHeader { get; set; }

    public IDictionary<string, dynamic> TemplateData { get; set; }

    public SendGridTrackingOption TrackingOption { get; set; } = new();
}

public class SendGridTrackingOption
{
    public bool AllowClick { get; set; }

    public bool AllowOpen { get; set; }

    public bool AllowGoogleAnalytics { get; set; }

    public bool AllowSubscription { get; set; }
}
