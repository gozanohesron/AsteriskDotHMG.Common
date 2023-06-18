namespace AsteriskDotHMG.Common.Helpers;

[AttributeUsage(AttributeTargets.Property)]
public class HideValueOnLoggerAttribute : Attribute
{
    public string Message { get; set; }

    public HideValueOnLoggerAttribute(string message)
    {
        Message = message;
    }
}
