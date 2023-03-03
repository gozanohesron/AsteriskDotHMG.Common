namespace AsteriskDotHMG.Notification.Models;

public class SendGridOptions
{
    public SendGridOptions()
    {

    }

    public SendGridOptions(string apiKey, string senderName, string senderEmail)
    {
        ApiKey = apiKey;
        SenderName = senderName;
        SenderEmail = senderEmail;
    }

    public string ApiKey { get; set; }
    
    public string SenderName { get; set; }

    public string SenderEmail { get; set; }
}
