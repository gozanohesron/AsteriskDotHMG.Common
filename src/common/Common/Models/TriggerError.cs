namespace AsteriskDotHMG.Common.Models;

public class TriggerError
{
    public object Data { get; set; }

    public string Message { get; set; }

    public string CorrelationId { get; set; }

    public List<ValidationError> Errors { get; set; } = new();
}
